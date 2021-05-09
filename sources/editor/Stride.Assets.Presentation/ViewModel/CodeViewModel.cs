// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Documents;
using System.Windows.Media;

using Microsoft.CodeAnalysis;

using Stride.Core.IO;
using Stride.Core.Extensions;
using Stride.Core.Presentation.Services;
using Stride.Core.Presentation.ViewModel;
using Stride.Core.Presentation.Dirtiables;
using Stride.Core.Assets;
using Stride.Core.Assets.Editor.ViewModel;
using Stride.Core.Translation;
using Stride.Assets.Scripts;
using Stride.Assets.Presentation.AssetEditors;
using Stride.Assets.Presentation.AssetEditors.ScriptEditor;

using RoslynWorkspace = Stride.Assets.Presentation.AssetEditors.ScriptEditor.RoslynWorkspace;

namespace Stride.Assets.Presentation.ViewModel
{
    /// <summary>
    ///   View model for source code project and files, including change tracking, Roslyn workspace updates, etc.
    /// </summary>
    public class CodeViewModel : DispatcherViewModel, IDisposable
    {
        /// <summary>
        ///   Minimum font size for the code editor.
        /// </summary>
        public const int MinimumEditorFontSize = 8;

        /// <summary>
        ///   Maximum font size for the code editor.
        /// </summary>
        public const int MaximumEditorFontSize = 72;

        private int editorFontSize = ScriptEditorSettings.FontSize.GetValue();   // Default size

        private readonly Brush keywordBrush;
        private readonly Brush typeBrush;

        /// <summary>
        ///   Gets the project watcher which tracks source code changes on the disk. It is created asychronously.
        /// </summary>
        public Task<ProjectWatcher> ProjectWatcher { get; }

        /// <summary>
        ///   Gets the Roslyn workspace. It is created asynchronously.
        /// </summary>
        public Task<RoslynWorkspace> Workspace { get; }

        /// <summary>
        ///   Gets or sets the current font size for the code editor. It will be saved in the settings.
        /// </summary>
        public int EditorFontSize
        {
            get => editorFontSize;
            set
            {
                if (value < MinimumEditorFontSize || value > MaximumEditorFontSize)
                    return;

                if (SetValue(ref editorFontSize, value))
                {
                    ScriptEditorSettings.FontSize.SetValue(editorFontSize);
                    ScriptEditorSettings.Save();
                }
            }
        }


        public CodeViewModel(StrideAssetsViewModel strideAssetsViewModel)
            : base(strideAssetsViewModel.SafeArgument(nameof(strideAssetsViewModel)).ServiceProvider)
        {
            ProjectWatcher = Task.Run(async () =>
            {
                var result = new ProjectWatcher(strideAssetsViewModel.Session);
                await result.Initialize();
                return result;
            });

            Workspace = ProjectWatcher.Result.RoslynHost.ContinueWith(roslynHost => roslynHost.Result.Workspace);

            Workspace = Workspace.ContinueWith(workspaceTask =>
            {
                var projectWatcher = ProjectWatcher.Result;
                var workspace = workspaceTask.Result;

                // Load and update roslyn workspace with latest compiled version
                foreach (var trackedAssembly in projectWatcher.TrackedAssemblies)
                {
                    var project = trackedAssembly.Project;
                    if (project != null)
                        workspace.AddOrUpdateProject(project);
                }

                void TrackedAssemblies_CollectionChanged(object sender, Core.Collections.TrackingCollectionChangedEventArgs e)
                {
                    var project = ((ProjectWatcher.TrackedAssembly)e.Item).Project;
                    if (project != null)
                    {
                        switch (e.Action)
                        {
                            case NotifyCollectionChangedAction.Add:
                            {
                                workspace.AddOrUpdateProject(project);
                                break;
                            }
                            case NotifyCollectionChangedAction.Remove:
                            {
                                workspace.RemoveProject(project.Id);
                                break;
                            }
                        }
                    }
                }
                projectWatcher.TrackedAssemblies.CollectionChanged += TrackedAssemblies_CollectionChanged;

                // TODO: Right now, we simply replace the solution with newly loaded one
                // Ideally, we should keep our existing solution and update it to follow external changes after initial loading (similar to VisualStudioWorkspace)
                // This should provide better integration with background changes and local changes
                projectWatcher.AssembliesChangedBroadcast.LinkTo(new ActionBlock<List<AssemblyChangedEvent>>(events =>
                {
                    if (events.Count == 0)
                        return;

                    Dispatcher.InvokeAsync(async () =>
                    {
                        // Update projects
                        foreach (var e in events.Where(x => x.ChangeType == AssemblyChangeType.Project))
                        {
                            var project = e.Project;
                            if (project != null)
                            {
                                await ReloadProject(strideAssetsViewModel.Session, project);
                            }
                        }

                        // Update files
                        foreach (var e in events.Where(x => x.ChangeType == AssemblyChangeType.Source))
                        {
                            var documentId = workspace.CurrentSolution.GetDocumentIdsWithFilePath(e.ChangedFile).FirstOrDefault();
                            if (documentId != null)
                                workspace.HostDocumentTextLoaderChanged(documentId, new FileTextLoader(e.ChangedFile, null));
                        }
                    }).Wait();
                }), new DataflowLinkOptions());

                return workspace;
            });

            // Apply syntax highlighting for tooltips
            keywordBrush = new SolidColorBrush(ClassificationHighlightColorsDark.KeywordColor);
            typeBrush = new SolidColorBrush(ClassificationHighlightColorsDark.TypeColor);
            keywordBrush.Freeze();
            typeBrush.Freeze();

            // TODO: Update with latest RoslynPad
            //SymbolDisplayPartExtensions.StyleRunFromSymbolDisplayPartKind = StyleRunFromSymbolDisplayPartKind;
            //SymbolDisplayPartExtensions.StyleRunFromTextTag = StyleRunFromTextTag;
        }



        /// <inheritdoc/>
        public override void Destroy()
        {
            EnsureNotDestroyed(nameof(CodeViewModel));
            Cleanup();
            base.Destroy();
        }

        /// <summary>
        ///   Reloads a project when a <c>.csproj</c> file changes on disk.
        /// </summary>
        /// <remarks>
        ///   In case of destructive changes (i.e. dirty files that disappeared), user confirmation will be asked to proceed.
        /// </remarks>
        private async Task ReloadProject(SessionViewModel session, Project project)
        {
            var workspace = await Workspace;

            // Get assets and namespace from csproj
            // TODO: Use roslyn list of files? not sure we could have non .cs files easily though
            // However, if we load from file, it might not be in sync with Roslyn state
            var projectFiles = Package.FindAssetsInProject(project.FilePath, out string projectNamespace);

            // Find associated ProjectViewModel
            if (!(session.LocalPackages.FirstOrDefault(y => y.Name == project.Name) is ProjectViewModel projectViewModel))
                return;

            // List current assets
            var projectAssets = new List<AssetViewModel>();
            var isProjectDirty = GetAssets(projectViewModel.Code, projectAssets);

            // Project is dirty, ask user if he really wants to auto-reload
            if (isProjectDirty)
            {
                var dialogResult = projectViewModel.Session.Dialogs.BlockingMessageBox(
                    string.Format(
                        Tr._p("Message", "Game Studio can't auto-reload the project file {0} because you have local changes such as new or deleted scripts.\r\n\r\nClick OK to keep reloading or Cancel to keep the current version."),
                        Path.GetFileName(project.FilePath)), MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.Cancel)
                    return;
            }

            // Remove deleted assets (and ask user if he really wants to proceed in case some of them were dirty?)
            bool continueReload = await DeleteRemovedProjectAssets(projectViewModel, projectAssets, project, projectFiles);
            if (!continueReload)
                return;

            // Update Roslyn project
            workspace.AddOrUpdateProject(project);

            // Add new assets
            AddNewProjectAssets(projectViewModel, projectAssets, project, projectFiles);

            // Mark project as non dirty
            // TODO: Does that work properly with Undo/Redo?
            UpdateDirtiness(projectViewModel.Code, false);
        }

        /// <summary>
        ///   Handles Script asset deletion (from Visual Studio/HDD external changes to Game Studio).
        /// </summary>
        /// <returns><c>false</c> if the user refused to continue (in case the deleted assets were dirty).</returns>
        private static async Task<bool> DeleteRemovedProjectAssets(ProjectViewModel projectViewModel, List<AssetViewModel> projectAssets, Project project, List<(UFile FilePath, UFile Link)> projectFiles)
        {
            // List IProjectAsset
            var currentProjectAssets = projectAssets.Where(x => x.AssetItem.Asset is IProjectAsset);

            var assetsToDelete = new List<AssetViewModel>();
            foreach (var asset in currentProjectAssets)
            {
                // NOTE: If file doesn't exist on HDD anymore (i.e. automatic csproj tracking for *.cs), no need to delete it anymore
                bool isDeleted = !projectFiles.Any(x => x.FilePath == asset.AssetItem.FullPath);
                if (isDeleted)
                {
                    assetsToDelete.Add(asset);
                }
            }

            var dirtyAssetsToDelete = assetsToDelete.Where(x => x.AssetItem.IsDirty).ToList();
            if (dirtyAssetsToDelete.Count > 0)
            {
                // Check if user is OK with deleting those dirty assets?
                var dialogResult = projectViewModel.Session.Dialogs.BlockingMessageBox(
                    string.Format(
                        Tr._p("Message", "The following source files in the {0} project have been deleted externally, but have unsaved changes in Game Studio. Do you want to delete these files?\r\n\r\n{1}"),
                       Path.GetFileName(project.FilePath), string.Join("\r\n", dirtyAssetsToDelete.Select(x => x.AssetItem.FullPath.ToWindowsPath()))),
                    MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.Cancel)
                    return false;
            }

            // Delete this asset
            if (assetsToDelete.Count > 0)
            {
                // TODO: This action (it should occur only during assembly releoad) will be undoable (undoing reload restores deleted script)
                if (!await projectViewModel.Session.ActiveAssetView.DeleteContent(assetsToDelete, true))
                    return false;
            }

            return true;
        }

        /// <summary>
        ///   Handles project asset addition (from Visual Studio/HDD external changes to Game Studio).
        /// </summary>
        private static void AddNewProjectAssets(ProjectViewModel projectViewModel, List<AssetViewModel> projectAssets, Project project, List<(UFile FilePath, UFile Link)> projectFiles)
        {
            // Nothing to add?
            if (projectFiles.Count == 0)
                return;

            var scriptAssets = projectAssets.Where(x => x.AssetItem.Asset is IProjectAsset).Select(x => x.AssetItem);

            var documentsToIgnore = (from scriptAsset in scriptAssets
                                     from document in projectFiles
                                     let ufileDoc = document.FilePath
                                     where ufileDoc == scriptAsset.FullPath
                                     select document).ToList();

            //remove what we have already
            var documentsCopy = new List<(UFile FilePath, UFile Link)>(projectFiles);
            foreach (var document in documentsToIgnore)
            {
                documentsCopy.Remove(document);
            }

            //add what we are missing
            if (documentsCopy.Count > 0)
            {
                var newScriptAssets = new List<AssetViewModel>();
                foreach (var document in documentsCopy)
                {
                    var docFile = new UFile(document.FilePath);
                    var projFile = new UFile(project.FilePath);

                    var assetName = docFile.MakeRelative(projectViewModel.Package.RootDirectory).GetDirectoryAndFileNameWithoutExtension();

                    var asset = new ScriptSourceFileAsset();
                    var assetItem = new AssetItem(assetName, asset)
                    {
                        IsDirty = true, //todo review / this is actually very important in the case of renaming, to propagate the change from VS to Game Studio, if we set it false here, during renaming the renamed asset won't be removed
                        SourceFolder = projectViewModel.Package.RootDirectory,
                    };

                    var directory = projectViewModel.GetOrCreateProjectDirectory(assetItem.Location.GetFullDirectory().FullPath, false);
                    var newScriptAsset = projectViewModel.CreateAsset(directory, assetItem, false, null);
                    newScriptAssets.Add(newScriptAsset);
                }

                // We're out of any transaction in this context so we have to manually notify that new assets were created.
                projectViewModel.Session.NotifyAssetPropertiesChanged(newScriptAssets);
            }
        }

        /// <summary>
        ///   Finds the assets in a directory and adds them to a list.
        /// </summary>
        /// <param name="dir">Directory.</param>
        /// <param name="list">List in which to add the assets.</param>
        /// <returns>Returns true if any of the (sub)directories is dirty.</returns>
        private static bool GetAssets(DirectoryBaseViewModel dir, List<AssetViewModel> list)
        {
            bool dirDirty = dir.IsDirty;

            foreach (var directory in dir.SubDirectories)
            {
                dirDirty |= GetAssets(directory, list);
            }

            //is dirty check is necessary to avoid unsaved scripts to be deleted
            list.AddRange(dir.Assets.Where(x => x.Asset is IProjectAsset));

            return dirDirty;
        }

        private void UpdateDirtiness(DirectoryBaseViewModel dir, bool isDirty)
        {
            if (dir.IsDirty)
                ((IDirtiable)dir).UpdateDirtiness(isDirty);

            foreach (var subdir in dir.SubDirectories)
            {
                UpdateDirtiness(subdir, isDirty);
            }
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            ProjectWatcher.Dispose();
        }

        private void StyleRunFromSymbolDisplayPartKind(SymbolDisplayPartKind partKind, Run run)
        {
            switch (partKind)
            {
                case SymbolDisplayPartKind.Keyword:
                    run.Foreground = keywordBrush;
                    return;

                case SymbolDisplayPartKind.StructName:
                case SymbolDisplayPartKind.EnumName:
                case SymbolDisplayPartKind.TypeParameterName:
                case SymbolDisplayPartKind.ClassName:
                case SymbolDisplayPartKind.DelegateName:
                case SymbolDisplayPartKind.InterfaceName:
                    run.Foreground = typeBrush;
                    return;
            }
        }

        private void StyleRunFromTextTag(string textTag, Run run)
        {
            switch (textTag)
            {
                case TextTags.Keyword:
                    run.Foreground = keywordBrush;
                    break;

                case TextTags.Struct:
                case TextTags.Enum:
                case TextTags.TypeParameter:
                case TextTags.Class:
                case TextTags.Delegate:
                case TextTags.Interface:
                    run.Foreground = typeBrush;
                    break;
            }
        }
    }
}
