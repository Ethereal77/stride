// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;
using Xenko.Assets.Presentation.AssetEditors.VisualScriptEditor;

namespace Xenko.Assets.Presentation.NodePresenters.Keys
{
    public static class VisualScriptData
    {
        public const string OwnerBlock = nameof(OwnerBlock);

        public static readonly PropertyKey<VisualScriptBlockViewModel> OwnerBlockKey = new PropertyKey<VisualScriptBlockViewModel>(OwnerBlock, typeof(VisualScriptData));
    }
}
