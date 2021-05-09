// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Stride.Core.Assets.Editor.ViewModel.CopyPasteProcessors;
using Stride.Assets.Entities;

namespace Stride.Assets.Presentation.ViewModel.CopyPasteProcessors
{
    internal class ScenePostPasteProcessor : AssetPostPasteProcessorBase<SceneAsset>
    {
        /// <inheritdoc />
        protected override void PostPasteDeserialization(SceneAsset asset)
        {
            // Clear all references (for now)
            asset.Parent = null;
            asset.ChildrenIds.Clear();
        }
    }
}
