// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets.Quantum.Internal;
using Stride.Core.Reflection;
using Stride.Core.Quantum;

namespace Stride.Core.Assets.Quantum.Tests.Helpers
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
