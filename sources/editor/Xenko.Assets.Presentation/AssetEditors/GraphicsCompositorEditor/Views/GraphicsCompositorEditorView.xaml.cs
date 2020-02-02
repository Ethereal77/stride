// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Xenko.Core.Assets.Editor.Services;
using Xenko.Core.Assets.Editor.ViewModel;
using Xenko.Assets.Presentation.AssetEditors.GraphicsCompositorEditor.ViewModels;
using Xenko.Assets.Presentation.ViewModel;

namespace Xenko.Assets.Presentation.AssetEditors.GraphicsCompositorEditor.Views
{
    /// <summary>
    /// Interaction logic for GraphicsCompositorEditorView.xaml
    /// </summary>
    public partial class GraphicsCompositorEditorView : IEditorView
    {
        private readonly TaskCompletionSource<bool> editorInitializationNotifier = new TaskCompletionSource<bool>();

        public GraphicsCompositorEditorView()
        {
            InitializeComponent();

            ZoomControl.AddHandler(MouseUpEvent, new MouseButtonEventHandler(ZoomControl_MouseUp), true);
        }

        /// <inheritdoc/>
        public Task EditorInitialization => editorInitializationNotifier.Task;

        public GraphicsCompositorGraph Graph { get; set; }
        /// <inheritdoc/>
        public async Task<IAssetEditorViewModel> InitializeEditor(AssetViewModel asset)
        {
            var graphicsCompositor = (GraphicsCompositorViewModel)asset;

            var editor = new GraphicsCompositorEditorViewModel(graphicsCompositor);
            Graph = new GraphicsCompositorGraph(editor.Blocks, editor.SelectedSharedRenderers, editor.SelectedRendererLinks);
            // Don't set the actual Editor property until the editor object is fully initialized - we don't want data bindings to access uninitialized properties
            var result = await editor.Initialize();
            editorInitializationNotifier.SetResult(result);
            if (result)
                return editor;

            editor.Destroy();
            return null;
        }

        private void ZoomControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Focus this element so that undo/redo and delete are properly handled after having any other window selected
            // and clicking on a vertex inside the graph (or any other non-empty space)
            Focus();

            // Mark event has unhandled so that context menu properly open (considered Handled when ZoomControl.Click is fired)
            e.Handled = false;
        }
    }
}
