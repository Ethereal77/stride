// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.UI.Events
{
    /// <summary>
    /// Indicates the routing strategy of a routed event.
    /// </summary>
    public enum RoutingStrategy
    {
        /// <summary>
        /// The routed event uses a bubbling strategy, where the event instance routes upwards through the tree, from event source to root.
        /// </summary>
        /// <userdoc>The routed event uses a bubbling strategy, where the event instance routes upwards through the tree, from event source to root.</userdoc>
        Bubble,
        /// <summary>
        /// The routed event uses a tunneling strategy, where the event instance routes downwards through the tree, from root to source element.
        /// </summary>
        /// <userdoc>The routed event uses a tunneling strategy, where the event instance routes downwards through the tree, from root to source element.</userdoc>
        Tunnel,
        /// <summary>
        /// The routed event does not route through an element tree, but does support other routed event capabilities such as class handling.
        /// </summary>
        /// <userdoc>The routed event does not route through an element tree, but does support other routed event capabilities such as class handling.</userdoc>
        Direct,
    }
}
