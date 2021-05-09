// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Compiler;
using Stride.Assets.Models;
using Stride.Assets.Presentation.Preview.Views;
using Stride.Editor.Preview;
using Stride.Engine;
using Stride.Rendering;

namespace Stride.Assets.Presentation.Preview
{
    /// <summary>
    /// An implementation of the <see cref="AssetPreview"/> that can preview materials.
    /// </summary>
    [AssetPreview(typeof(ProceduralModelAsset), typeof(ModelPreviewView))]
    public class ProceduralModelPreview : PreviewFromEntity<ProceduralModelAsset>
    {
        /// <inheritdoc/>
        protected override PreviewEntity CreatePreviewEntity()
        {
            // load the material from the data base
            var model = LoadAsset<Model>(AssetItem.Location);

            // create the entity, create and set the model component
            var modelEntity = new Entity { Name = BuildName() };
            modelEntity.Add(new ModelComponent { Model = model });

            var previewEntity = new PreviewEntity(modelEntity);
            previewEntity.Disposed += () => UnloadAsset(model);
            return previewEntity;
        }
    }
}
