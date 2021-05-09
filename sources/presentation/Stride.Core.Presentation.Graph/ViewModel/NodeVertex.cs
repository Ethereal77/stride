// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

using GraphX.Controls;
using GraphX.PCL.Common.Models;

namespace Stride.Core.Presentation.Graph.ViewModel
{
    /// <summary>
    /// Base vertex used for node-based graphs. 
    /// This class must derived from VertexBase in GraphX.
    /// </summary>
    public class NodeVertex : VertexBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public NodeVertex() { /* nothing */ }

        public virtual void AddOutgoing(NodeVertex target, object from, object to)
        {
        }

        public virtual void ConnectControl(VertexControl vertexControl)
        {
        }

        public virtual void DisconnectControl(VertexControl vertexControl)
        {
        }

        /// <summary>
        /// Collection of input slots
        /// </summary>
        public virtual ObservableCollection<object> InputSlots { get; set; }

        /// <summary>
        /// Collection of output slots
        /// </summary>
        public virtual ObservableCollection<object> OutputSlots { get; set; }
    }
}
