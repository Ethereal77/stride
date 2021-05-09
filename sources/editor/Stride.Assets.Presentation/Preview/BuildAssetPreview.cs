// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Core.Assets.Editor.Services;
using Stride.Core.BuildEngine;
using Stride.Core.Extensions;
using Stride.Core.MicroThreading;
using Stride.Core.Serialization.Contents;
using Stride.Editor.Build;
using Stride.Editor.Preview;

namespace Stride.Assets.Presentation.Preview
{
    /// <summary>
    /// An implementation of the <see cref="AssetPreview"/> class that provide utilities to build an asset. 
    /// This class can be inherited to make preview class for assets that requires asset builds.
    /// </summary>
    /// <typeparam name="T">The type of asset this preview can display.</typeparam>
    public abstract class BuildAssetPreview<T> : AssetPreview<T> where T : Asset
    {
        private readonly Guid previewContextId = Guid.NewGuid();

        /// <summary>
        /// The output objects generated by the build of the associated asset, when the build succeeded.
        /// </summary>
        protected IReadOnlyDictionary<ObjectUrl, OutputObject> OutputObjects;

        /// <summary>
        /// Compiles the assets needed for this preview.
        /// </summary>
        /// <remarks>The default implementation compile the related assets and its dependencies.</remarks>
        /// <returns>An instance of the <see cref="AssetCompilerResult"/> class containing the generated build steps.</returns>
        protected virtual AssetCompilerResult Compile()
        {
            return Builder.Compile(AssetItem);
        }

        /// <summary>
        /// Load an asset to the preview.
        /// </summary>
        /// <typeparam name="TAssetType">The type of the asset to load</typeparam>
        /// <param name="url">The path to the asset to load</param>
        /// <param name="settings">The settings. If null, fallback to <see cref="ContentManagerLoaderSettings.Default" />.</param>
        /// <returns>The loaded asset</returns>
        public TAssetType LoadAsset<TAssetType>(string url, ContentManagerLoaderSettings settings = null) where TAssetType : class
        {
            TAssetType result = null;

            try
            {
                // This method can be invoked both from a script and from a regular task. In the second case, it will use the out-of-microthread database which need to be locked.
                // TODO: Ensure this method is always called from the preview game (it is not at least when a property is modified, currently), so we don't need to lock. Note: should be the case now, assume it is after GDC!
                if (Scheduler.CurrentMicroThread == null)
                    Monitor.Enter(AssetBuilderService.OutOfMicrothreadDatabaseLock);

                MicrothreadLocalDatabases.MountDatabase(OutputObjects.Yield());

                try
                {
                    result = Game.Content.Load<TAssetType>(url, settings);
                }
                finally
                {
                    if (Scheduler.CurrentMicroThread == null)
                    {
                        MicrothreadLocalDatabases.UnmountDatabase();
                    }
                }
            }
            catch (Exception e)
            {
                Builder.Logger.Error($"An exception was triggered when trying to load the entity [{url}] for the preview of asset item [{AssetItem.Location}].", e);
            }
            finally
            {
                if (Scheduler.CurrentMicroThread == null)
                    Monitor.Exit(AssetBuilderService.OutOfMicrothreadDatabaseLock);
            }
            return result;
        }

        /// <summary>
        /// Unload an asset from the preview.
        /// </summary>
        /// <param name="asset">The asset to unload</param>
        public void UnloadAsset(object asset)
        {
            if (asset != null)
                Game.Content.Unload(asset);
        }


        protected override async Task<bool> PrepareContent()
        {
            return await BuildAsset();
        }

        protected async Task<bool> BuildAsset()
        {
            // get the build step required by the preview builder
            AssetCompilerResult compilationResult = null;

            var buildUnit = new AnonymousAssetBuildUnit(new AssetBuildUnitIdentifier(previewContextId, Asset.Id), () => { compilationResult = Compile(); return compilationResult.BuildSteps; });
            buildUnit.PriorityMajor = DefaultAssetBuilderPriorities.PreviewPriority;
            Builder.AssetBuilderService.PushBuildUnit(buildUnit);

            await buildUnit.Wait();

            UpdateBuildAssetResults(buildUnit, compilationResult);
            return buildUnit.Succeeded;
        }

        protected virtual void UpdateBuildAssetResults(AnonymousAssetBuildUnit buildUnit, AssetCompilerResult compilationResult)
        {
            if (buildUnit.Succeeded)
            {
                // If successful, store the output objects so they can be mounted on a database when needed.
                OutputObjects = compilationResult.BuildSteps.OutputObjects;
            }
        }
    }
}
