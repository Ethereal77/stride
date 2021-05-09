// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;
using Stride.Core.Reflection;
using Stride.Core.Quantum;

namespace Stride.Core.Assets.Quantum
{
    public interface IAssetMemberNode : IAssetNode, IMemberNode
    {
        bool IsNonIdentifiableCollectionContent { get; }

        bool CanOverride { get; }

        [NotNull]
        new IAssetObjectNode Parent { get; }

        new IAssetObjectNode Target { get; }

        void OverrideContent(bool isOverridden);

        OverrideType GetContentOverride();

        bool IsContentOverridden();

        bool IsContentInherited();
    }
}
