// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using GraphX;
using GraphX.PCL.Logic.Models;

using QuickGraph;

namespace Stride.Core.Presentation.Graph.ViewModel
{
    /// <summary>
    /// Logics core object which contains all algorithms and logic settings
    /// </summary>
    public class NodeGraphLogicCore : GXLogicCore<NodeVertex, NodeEdge, BidirectionalGraph<NodeVertex, NodeEdge>> { }
}
