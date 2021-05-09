// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core;
using Stride.Assets.Models;

namespace Stride.Assets.Presentation.NodePresenters.Keys
{
    public static class ModelNodeLinkData
    {
        public const string AvailableNodes = nameof(AvailableNodes);
        public static readonly PropertyKey<IEnumerable<NodeInformation>> Key = new PropertyKey<IEnumerable<NodeInformation>>(AvailableNodes, typeof(ModelNodeLinkData));
    }
}
