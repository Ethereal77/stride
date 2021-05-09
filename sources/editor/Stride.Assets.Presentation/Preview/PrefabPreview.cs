// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Assets.Entities;
using Stride.Assets.Presentation.Preview.Views;
using Stride.Editor.Preview;
using Stride.Engine;

namespace Stride.Assets.Presentation.Preview
{
    [AssetPreview(typeof(PrefabAsset), typeof(ModelPreviewView))]
    public class PrefabPreview : PreviewFromEntity<PrefabAsset>
    {
        protected override PreviewEntity CreatePreviewEntity()
        {
            var prefab = LoadAsset<Prefab>(AssetItem.Location);

            var entity = new Entity { Name = "Preview Entity of model: " + AssetItem.Location };

            foreach (var prefabEntity in prefab.Entities)
            {
                entity.AddChild(prefabEntity);
            }

            var previewEntity = new PreviewEntity(entity);
            previewEntity.Disposed += () => UnloadAsset(prefab);

            return previewEntity;
        }
    }
}
