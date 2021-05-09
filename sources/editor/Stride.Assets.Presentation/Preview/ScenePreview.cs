// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Assets.Entities;
using Stride.Assets.Presentation.Preview.Views;
using Stride.Editor.Preview;

namespace Stride.Assets.Presentation.Preview
{
    [AssetPreview(typeof(SceneAsset), typeof(ScenePreviewView))]
    public class ScenePreview : AssetPreview<SceneAsset>
    {
    }
}
