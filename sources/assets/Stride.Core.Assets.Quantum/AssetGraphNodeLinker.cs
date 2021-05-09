// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Annotations;
using Stride.Core.Quantum;

namespace Stride.Core.Assets.Quantum
{
    public class AssetGraphNodeLinker : GraphNodeLinker
    {
        private readonly AssetPropertyGraphDefinition propertyGraphDefinition;

        public AssetGraphNodeLinker(AssetPropertyGraphDefinition propertyGraphDefinition)
        {
            this.propertyGraphDefinition = propertyGraphDefinition;
        }

        protected override bool ShouldVisitMemberTarget([NotNull] IMemberNode member)
        {
            return !propertyGraphDefinition.IsMemberTargetObjectReference(member, member.Retrieve()) && base.ShouldVisitMemberTarget(member);
        }

        protected override bool ShouldVisitTargetItem([NotNull] IObjectNode collectionNode, NodeIndex index)
        {
            return !propertyGraphDefinition.IsTargetItemObjectReference(collectionNode, index, collectionNode.Retrieve(index)) && base.ShouldVisitTargetItem(collectionNode, index);
        }
    }
}
