// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Extensions;
using Stride.Core.Mathematics;
using Stride.Core.Threading;
using Stride.Core.Assets.Editor.Services;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Animations;
using Stride.Assets.Presentation.CurveEditor.ViewModels;
using Stride.Assets.Presentation.CurveEditor.Views;

using AvalonDock.Layout;

namespace Stride.GameStudio
{
    internal sealed class AssetEditorsManager : IAssetEditorsManager, IDestroyable
    {
        private readonly Dictionary<IAssetEditorViewModel, LayoutAnchorable> assetEditors = new Dictionary<IAssetEditorViewModel, LayoutAnchorable>();
        private readonly HashSet<AssetViewModel> openedAssets = new HashSet<AssetViewModel>();
        // TODO: Have a base interface for all editors and factorize to make curve editor not be a special case anymore
        private Tuple<CurveEditorViewModel, LayoutAnchorable> curveEditor;

        private readonly AsyncLock mutex = new AsyncLock();
        private readonly DockingLayoutManager dockingLayoutManager;
        private readonly SessionViewModel session;

        public AssetEditorsManager([NotNull] DockingLayoutManager dockingLayoutManager, [NotNull] SessionViewModel session)
        {
            if (dockingLayoutManager is null)
                throw new ArgumentNullException(nameof(dockingLayoutManager));
            if (session is null)
                throw new ArgumentNullException(nameof(session));

            this.dockingLayoutManager = dockingLayoutManager;
            this.session = session;
            session.DeletedAssetsChanged += AssetsDeleted;
        }

        /// <summary>
        ///   Gets a list of the assets that are currently opened in an editor.
        /// </summary>
        /// <remarks>
        ///   This does not include all assets in <see cref="IMultipleAssetEditorViewModel"/> but rather those that were explicitly opened.
        /// </remarks>
        public IReadOnlyCollection<AssetViewModel> OpenedAssets => openedAssets;

        /// <inheritdoc />
        void IDestroyable.Destroy()
        {
            session.DeletedAssetsChanged -= AssetsDeleted;
        }

        /// <inheritdoc/>
        public void OpenCurveEditorWindow([NotNull] object curve, string name)
        {
            if (curve is null)
                throw new ArgumentNullException(nameof(curve));
            if (dockingLayoutManager is null)
                throw new InvalidOperationException("This method can only be invoked on the IEditorDialogService that has the editor main window as parent.");

            CurveEditorViewModel editorViewModel = null;
            LayoutAnchorable editorPane = null;

            if (curveEditor != null)
            {
                // Curve editor already exists
                editorViewModel = curveEditor.Item1;
                editorPane = curveEditor.Item2;
            }

            // Create the editor view model if needed
            if (editorViewModel is null)
            {
                editorViewModel = new CurveEditorViewModel(session.ServiceProvider, session);
            }

            // Populate the editor view model
            if (curve is IComputeCurve<Color4> color4Curve)
            {
                editorViewModel.AddCurve(color4Curve, name);
            }
            else if (curve is IComputeCurve<float> floatCurve)
            {
                editorViewModel.AddCurve(floatCurve, name);
            }
            else if (curve is IComputeCurve<Quaternion> quaternionCurve)
            {
                editorViewModel.AddCurve(quaternionCurve, name);
            }
            else if (curve is IComputeCurve<Vector2> vector2Curve)
            {
                editorViewModel.AddCurve(vector2Curve, name);
            }
            else if (curve is IComputeCurve<Vector3> vector3Curve)
            {
                editorViewModel.AddCurve(vector3Curve, name);
            }
            else if (curve is IComputeCurve<Vector4> vector4Curve)
            {
                editorViewModel.AddCurve(vector4Curve, name);
            }

            editorViewModel.Focus();

            // Create the editor pane if needed
            if (editorPane is null)
            {
                editorPane = new LayoutAnchorable
                {
                    Content = new CurveEditorView { DataContext = editorViewModel },
                    Title = "Curve Editor",
                    CanClose = true,
                };

                editorPane.Closed += CurveEditorClosed;

                //editorPane.Closed += (s, e) =>
                //{
                //    if (((LayoutAnchorable)s).IsHidden)
                //    {
                //        RemoveCurveEditor(true);
                //    }
                //};

                AvalonDockHelper.GetDocumentPane(dockingLayoutManager.DockingManager).Children.Add(editorPane);
            }

            curveEditor = Tuple.Create(editorViewModel, editorPane);

            MakeActiveVisible(editorPane);
        }

        /// <inheritdoc/>
        public void CloseCurveEditorWindow()
        {
            RemoveCurveEditor(true);
        }

        private void RemoveCurveEditor(bool removePane)
        {
            if (curveEditor is null)
                return;

            var editor = curveEditor.Item1;
            var pane = curveEditor.Item2;
            curveEditor = null;
            // Clean view model
            editor.Destroy();

            CleanEditorPane(pane);
            if (removePane)
            {
                RemoveEditorPane(pane);
            }
        }

        private void CurveEditorClosed(object sender, EventArgs eventArgs)
        {
            RemoveCurveEditor(true);
        }

        /// <inheritdoc/>
        [NotNull]
        public Task OpenAssetEditorWindow([NotNull] AssetViewModel asset)
        {
            return OpenAssetEditorWindow(asset, true);
        }

        /// <inheritdoc/>
        public bool CloseAllEditorWindows(bool? save)
        {
            // Attempt to close all opened assets
            if (!openedAssets.ToList().All(asset => CloseAssetEditorWindow(asset, save)))
                return false;

            // Then check that they are no remaining editor
            if (assetEditors.Count > 0)
            {
                // Nicolas: this case should not happen. Please let me know if it happens to you.
                // NOTE: This likely means that some editors leaked (esp. in the case of multi-asset editors), but force removing should be enough.
                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();

                assetEditors.Keys.ToList().ForEach(RemoveEditor);
            }

            CloseCurveEditorWindow();

            return true;
        }

        /// <inheritdoc/>
        public void CloseAllHiddenWindows()
        {
            foreach (var pane in AvalonDockHelper.GetAllAnchorables(dockingLayoutManager.DockingManager).Where(p => string.IsNullOrEmpty(p.ContentId) && p.IsHidden).ToList())
            {
                CleanEditorPane(pane);
                RemoveEditorPane(pane);
            }
        }

        /// <inheritdoc/>
        public bool CloseAssetEditorWindow([NotNull] AssetViewModel asset, bool? save)
        {
            var canClose = asset.Editor?.PreviewClose(save) ?? true;
            if (canClose)
                CloseEditorWindow(asset);

            return canClose;
        }

        /// <inheritdoc/>
        public void HideAllAssetEditorWindows()
        {
            foreach (var editorPane in assetEditors.Values)
            {
                editorPane.Hide();
            }
        }

        /// <summary>
        ///   Retrieves the list of all assets that are currently opened in an editor.
        /// </summary>
        /// <returns>A list of all assets currently opened.</returns>
        /// <remarks>
        ///   This includes all assets in <see cref="IMultipleAssetEditorViewModel"/> even those that were not explicitly opened.
        /// </remarks>
        /// <seealso cref="OpenedAssets"/>
        [NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IReadOnlyCollection<AssetViewModel> GetCurrentlyOpenedAssets()
        {
            var hashSet = new HashSet<AssetViewModel>(openedAssets);
            assetEditors.Keys.OfType<IMultipleAssetEditorViewModel>().ForEach(x => hashSet.AddRange(x.OpenedAssets));
            return hashSet;
        }

        /// <summary>
        ///   Opens (and activates) an editor window for the given asset. If an editor window for this asset already exists, simply activates it.
        /// </summary>
        /// <param name="asset">The asset for which to show an editor window.</param>
        /// <param name="saveSettings">
        ///   <c>true</c> if <see cref="MRUAdditionalData.OpenedAssets"/> should be updated; <c>false</c> otherwise. Note that if the
        ///   editor fails to load it will not be updated.
        /// </param>
        /// <returns>A <see cref="Task"/> that can be awaited.</returns>
        internal async Task OpenAssetEditorWindow([NotNull] AssetViewModel asset, bool saveSettings)
        {
            if (asset is null)
                throw new ArgumentNullException(nameof(asset));
            if (dockingLayoutManager is null)
                throw new InvalidOperationException("This method can only be invoked on the IEditorDialogService that has the editor main window as parent.");

            // Switch to the editor layout before adding any Pane
            if (assetEditors.Count == 0)
            {
                dockingLayoutManager.SwitchToEditorLayout();
            }

            using (await mutex.LockAsync())
            {
                LayoutAnchorable editorPane = null;
                IEditorView view;

                // Asset already has an editor?
                if (asset.Editor != null)
                {
                    // Look for the corresponding pane
                    if (!assetEditors.TryGetValue(asset.Editor, out editorPane))
                    {
                        // Inconsistency, clean leaking editor
                        RemoveAssetEditor(asset);
                        // Try to find if another editor currently has this asset
                        var editor = assetEditors.Keys.OfType<IMultipleAssetEditorViewModel>().FirstOrDefault(x => x.OpenedAssets.Contains(asset));
                        if (editor != null)
                        {
                            editorPane = assetEditors[editor];
                            asset.Editor = editor;
                        }
                    }
                }

                // Existing editor?
                if (editorPane != null)
                {
                    // Make the pane visible immediately
                    MakeActiveVisible(editorPane);
                    view = editorPane.Content as IEditorView;
                    if (view?.EditorInitialization != null)
                    {
                        // Wait for the end of the initialization
                        await view.EditorInitialization;
                    }
                    return;
                }

                // Create a new editor view
                view = asset.ServiceProvider.Get<IAssetsPluginService>().ConstructEditionView(asset);
                if (view != null)
                {
                    // Pane may already exists (e.g. created from layout saving)
                    editorPane = AvalonDockHelper.GetAllAnchorables(dockingLayoutManager.DockingManager).FirstOrDefault(p => p.Title == asset.Url);
                    if (editorPane is null)
                    {
                        editorPane = new LayoutAnchorable { CanClose = true };
                        // Stack the asset in the dictionary of editor to prevent double-opening while double-clicking twice on the asset, since the initialization is async
                        AvalonDockHelper.GetDocumentPane(dockingLayoutManager.DockingManager).Children.Add(editorPane);
                    }
                    editorPane.IsActiveChanged += EditorPaneIsActiveChanged;
                    editorPane.IsSelectedChanged += EditorPaneIsSelectedChanged;
                    editorPane.Closing += EditorPaneClosing;
                    editorPane.Closed += EditorPaneClosed;
                    editorPane.Content = view;
                    // Make the pane visible immediately
                    MakeActiveVisible(editorPane);
                    // Initialize the editor view
                    view.DataContext = asset;

                    // Create a binding for the title
                    var binding = new Binding(nameof(AssetViewModel.Url)) { Mode = BindingMode.OneWay, Source = asset };
                    BindingOperations.SetBinding(editorPane, LayoutContent.TitleProperty, binding);

                    var viewModel = await view.InitializeEditor(asset);
                    if (viewModel is null)
                    {
                        // Could not initialize editor
                        CleanEditorPane(editorPane);
                        RemoveEditorPane(editorPane);
                    }
                    else
                    {
                        assetEditors[viewModel] = editorPane;
                        openedAssets.Add(asset);
                        if (viewModel is IMultipleAssetEditorViewModel multiEditor)
                        {
                            foreach (var item in multiEditor.OpenedAssets)
                            {
                                if (item.Editor != null)
                                {
                                    // NOTE: This could happen in some case after undo/redo that involves parenting of scenes
                                    RemoveAssetEditor(item);
                                }
                                item.Editor = viewModel;
                            }
                            multiEditor.OpenedAssets.CollectionChanged += (_, e) => MultiEditorOpenAssetsChanged(multiEditor, e);
                        }
                        else
                        {
                            asset.Editor = viewModel;
                        }
                    }
                }
            }

            // If the opening of the editor failed, go back to normal layout
            if (assetEditors.Count == 0)
            {
                dockingLayoutManager.SwitchToNormalLayout();
                return;
            }

            if (saveSettings)
            {
                dockingLayoutManager.SaveOpenAssets(OpenedAssets);
            }
        }

        private void CloseEditorWindow([NotNull] AssetViewModel asset)
        {
            // Make asset view active
            asset.Session.ActiveProperties = asset.Session.AssetViewProperties;
            // Remove editor
            RemoveAssetEditor(asset);
            // If no more editor open, change layout
            if (assetEditors.Count == 0)
            {
                dockingLayoutManager.SwitchToNormalLayout();
            }
        }

        private void MultiEditorOpenAssetsChanged([NotNull] IMultipleAssetEditorViewModel multiEditor, [NotNull] NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems?.Count > 0)
                    {
                        foreach (AssetViewModel item in e.OldItems)
                        {
                            item.Editor = null;
                        }
                    }
                    if (e.NewItems?.Count > 0)
                    {
                        foreach (AssetViewModel item in e.NewItems)
                        {
                            if (item.Editor != null && assetEditors.ContainsKey(item.Editor))
                            {
                                RemoveAssetEditor(item);
                            }
                            item.Editor = multiEditor;
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    // Nothing to do
                    break;

                case NotifyCollectionChangedAction.Reset:
                    throw new InvalidOperationException();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///   Removes the editor for the given asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        private void RemoveAssetEditor([NotNull] AssetViewModel asset)
        {
            openedAssets.Remove(asset);

            var editor = asset.Editor;
            if (editor is null)
                return;

            asset.Editor = null;
            RemoveEditor(editor);
        }

        private void RemoveEditor([NotNull] IAssetEditorViewModel editor)
        {
            assetEditors.TryGetValue(editor, out LayoutAnchorable editorPane);

            if (editor is IMultipleAssetEditorViewModel multiEditor)
            {
                multiEditor.OpenedAssets.CollectionChanged -= (_, e) => MultiEditorOpenAssetsChanged(multiEditor, e);
                // Clean asset view models
                foreach (var item in multiEditor.OpenedAssets)
                {
                    item.Editor = null;
                }
            }
            // Remove editor
            assetEditors.Remove(editor);
            // Attempt to destroy the editor
            try
            {
                editor.Destroy();
            }
            catch (ObjectDisposedException) { }

            // Clean and remove editor pane
            if (editorPane != null)
            {
                CleanEditorPane(editorPane);
                RemoveEditorPane(editorPane);
            }
        }

        private void AssetsDeleted(object sender, [NotNull] NotifyCollectionChangedEventArgs e)
        {
            e.NewItems?.Cast<AssetViewModel>().Where(x => openedAssets.Contains(x)).ForEach(CloseEditorWindow);
        }

        /// <summary>
        ///   Cleans the editor pane.
        /// </summary>
        /// <param name="editorPane">The editor pane.</param>
        /// <seealso cref="RemoveEditorPane"/>
        private static void CleanEditorPane([NotNull] LayoutAnchorable editorPane)
        {
            // Destroy the editor view
            (editorPane.Content as IDestroyable)?.Destroy();
            editorPane.Content = null;
            editorPane.Title = null;
        }

        /// <summary>
        ///   Removes the editor pane.
        /// </summary>
        /// <param name="editorPane">The editor pane.</param>
        /// <seealso cref="CleanEditorPane"/>
        private void RemoveEditorPane([NotNull] LayoutAnchorable editorPane)
        {
            editorPane.IsActiveChanged -= EditorPaneIsActiveChanged;
            editorPane.IsSelectedChanged -= EditorPaneIsSelectedChanged;
            editorPane.Closing -= EditorPaneClosing;
            editorPane.Closed -= EditorPaneClosed;

            // If this editor pane was closed by user, no need to do that; it is necessary for closing programmatically
            if (editorPane.Root != null)
                editorPane.Close();
        }

        private static void EditorPaneClosing(object sender, CancelEventArgs e)
        {
            var editorPane = (LayoutAnchorable) sender;

            var element = editorPane.Content as FrameworkElement;
            var asset = element?.DataContext as AssetViewModel;

            // If any editor couldn't close, cancel the sequence
            if (!(asset?.Editor?.PreviewClose(null) ?? true))
            {
                e.Cancel = true;
            }
        }

        private void EditorPaneClosed(object sender, EventArgs eventArgs)
        {
            var editorPane = (LayoutAnchorable)sender;

            var element = editorPane.Content as FrameworkElement;
            if (element?.DataContext is AssetViewModel asset)
            {
                CloseEditorWindow(asset);
            }
        }

        private static void EditorPaneContentLoaded(object sender, RoutedEventArgs e)
        {
            // Give focus to element
            var element = (FrameworkElement) sender;
            if (!element.IsKeyboardFocusWithin)
                Keyboard.Focus(element);
        }

        private static void EditorPaneIsActiveChanged(object sender, EventArgs e)
        {
            var editorPane = (LayoutAnchorable) sender;

            if (editorPane.Content is FrameworkElement element)
            {
                if (editorPane.IsActive)
                {
                    if (element.IsLoaded)
                    {
                        // Give focus to element
                        if (!element.IsKeyboardFocusWithin)
                            Keyboard.Focus(element);
                    }
                    else
                    {
                        // Not loaded yet, let's defer the focus until loaded
                        element.Loaded += EditorPaneContentLoaded;
                    }
                }
                else
                {
                    element.Loaded -= EditorPaneContentLoaded;
                }
            }
        }

        private static void EditorPaneIsSelectedChanged(object sender, EventArgs e)
        {
            var editorPane = (LayoutAnchorable) sender;

            if (editorPane.Content is FrameworkElement element)
            {
                var assetViewModel = element?.DataContext as AssetViewModel;
                if (assetViewModel?.Editor is Assets.Presentation.AssetEditors.GameEditor.ViewModels.GameEditorViewModel gameEditor)
                {
                    // A tab/sub-window is visible via IsSelected, not IsVisible
                    if (editorPane.IsSelected)
                    {
                        gameEditor.ShowGame();
                    }
                    else
                    {
                        gameEditor.HideGame();
                    }
                }
            }
        }

        /// <summary>
        ///   Makes an editor pane active and visible.
        /// </summary>
        /// <param name="editorPane">Editor pane.</param>
        private static void MakeActiveVisible([NotNull] LayoutAnchorable editorPane)
        {
            editorPane.IsActive = true;
            editorPane.Show();
        }
    }
}
