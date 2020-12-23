// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Core.Serialization;
using Stride.Engine;

namespace Stride.Assets.Entities.ComponentChecks
{
    /// <summary>
    ///   Represents a check that verifies if a <see cref="ModelComponent"/> has a <see cref="Stride.Rendering.Model"/>
    ///   associated with it and that it has a reachable <see cref="Asset"/>.
    /// </summary>
    public class ModelComponentCheck : IEntityComponentCheck
    {
        /// <inheritdoc/>
        public bool AppliesTo(Type componentType)
        {
            return componentType == typeof(ModelComponent);
        }

        /// <inheritdoc/>
        public void Check(EntityComponent component, Entity entity, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result)
        {
            var modelComponent = component as ModelComponent;
            if (modelComponent.Model is null)
            {
                result.Warning($"The entity [{targetUrlInStorage}:{entity.Name}] has a model component that does not reference any model.");
            }
            else
            {
                var modelAttachedReference = AttachedReferenceManager.GetAttachedReference(modelComponent.Model);
                var modelId = modelAttachedReference.Id;

                // Compute the full path to the source asset
                var modelAssetItem = assetItem.Package.Session.FindAsset(modelId);
                if (modelAssetItem is null)
                {
                    result.Error($"The entity [{targetUrlInStorage}:{entity.Name}] is referencing an unreachable model.");
                }
            }
        }
    }
}
