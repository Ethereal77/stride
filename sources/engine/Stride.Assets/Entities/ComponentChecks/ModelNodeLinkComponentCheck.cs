// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Assets;
using Stride.Core.Assets.Compiler;
using Stride.Engine;

namespace Stride.Assets.Entities.ComponentChecks
{
    /// <summary>
    ///   Represents a check that verifies the validity of a <see cref="ModelNodeLinkComponent"/>.
    /// </summary>
    public class ModelNodeLinkComponentCheck : IEntityComponentCheck
    {
        /// <inheritdoc/>
        public bool AppliesTo(Type componentType)
        {
            return componentType == typeof(ModelNodeLinkComponent);
        }

        /// <inheritdoc/>
        public void Check(EntityComponent component, Entity entity, AssetItem assetItem, string targetUrlInStorage, AssetCompilerResult result)
        {
            var nodeLinkComponent = component as ModelNodeLinkComponent;
            nodeLinkComponent.ValidityCheck();
            if (!nodeLinkComponent.IsValid)
            {
                result.Warning($"The Model Node Link between {entity.Name} and {nodeLinkComponent.Target?.Entity.Name} is invalid.");
                nodeLinkComponent.Target = null;
            }
        }
    }
}
