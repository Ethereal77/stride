// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using Stride.Core.BuildEngine;
using Stride.Core.Serialization.Contents;
using Stride.Animations;
using Stride.Rendering;

namespace Stride.Assets.Models
{
    [Description("Import Assimp")]
    public class ImportAssimpCommand : ImportModelCommand
    {
        private static readonly string[] supportedExtensions = AssimpAssetImporter.FileExtensions.Split(';');

        /// <inheritdoc/>
        public override string Title
        {
            get { string title = "Import Assimp "; try { title += Path.GetFileName(SourcePath) ?? "[File]"; } catch { title += "[INVALID PATH]"; } return title; }
        }

        public static bool IsSupportingExtensions(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return false;

            var extToLower = ext.ToLowerInvariant();

            return supportedExtensions.Any(supExt => supExt.Equals(extToLower));
        }

        private Importer.AssimpNET.MeshConverter CreateMeshConverter(ICommandContext commandContext)
        {
            return new Importer.AssimpNET.MeshConverter(commandContext.Logger)
            {
                AllowUnsignedBlendIndices = AllowUnsignedBlendIndices
            };
        }

        protected override Model LoadModel(ICommandContext commandContext, ContentManager contentManager)
        {
            var converter = CreateMeshConverter(commandContext);

            // NOTE: FBX exporter uses Materials for the mapping, but Assimp already uses indices so we can reuse them
            // We should still unify the behavior to be more consistent at some point (i.e. if model was changed on the HDD but not in the asset).
            // This should probably be better done during a large-scale FBX/Assimp refactoring.
            var sceneData = converter.Convert(SourcePath, Location, DeduplicateMaterials);
            return sceneData;
        }

        protected override Dictionary<string, AnimationClip> LoadAnimation(ICommandContext commandContext, ContentManager contentManager, out TimeSpan duration)
        {
            var meshConverter = this.CreateMeshConverter(commandContext);
            var sceneData = meshConverter.ConvertAnimation(SourcePath, Location);

            duration = sceneData.Duration;
            return sceneData.AnimationClips;
        }

        protected override Skeleton LoadSkeleton(ICommandContext commandContext, ContentManager contentManager)
        {
            var meshConverter = this.CreateMeshConverter(commandContext);
            var sceneData = meshConverter.ConvertSkeleton(SourcePath, Location);
            return sceneData;
        }

        public override string ToString()
        {
            return "Import Assimp " + base.ToString();
        }
    }
}
