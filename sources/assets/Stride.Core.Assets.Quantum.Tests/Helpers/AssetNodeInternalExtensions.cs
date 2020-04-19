// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets.Quantum.Internal;
using Xenko.Core.Reflection;
using Xenko.Core.Quantum;

namespace Xenko.Core.Assets.Quantum.Tests.Helpers
{
    public static class AssetNodeInternalExtensions
    {
        public static OverrideType GetItemOverride(this IAssetNode node, NodeIndex index)
        {
            return ((IAssetObjectNodeInternal)node).GetItemOverride(index);
        }

        public static OverrideType GetKeyOverride(this IAssetNode node, NodeIndex index)
        {
            return ((IAssetObjectNodeInternal)node).GetKeyOverride(index);
        }
    }
}
