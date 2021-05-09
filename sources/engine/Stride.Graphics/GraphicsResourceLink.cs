// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Graphics
{
    /// <summary>
    ///   Represents a resource allocated by <see cref="GraphicsResourceAllocator"/> providing allocation information.
    /// </summary>
    public sealed class GraphicsResourceLink
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="GraphicsResourceLink"/> class.
        /// </summary>
        /// <param name="resource">The graphics resource.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resource"/> is a <c>null</c> reference.</exception>
        internal GraphicsResourceLink(GraphicsResourceBase resource)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }


        /// <summary>
        ///   Gets the graphics resource.
        /// </summary>
        public GraphicsResourceBase Resource { get; }

        /// <summary>
        ///   Gets the last time this resource was accessed.
        /// </summary>
        /// <value>The last access time.</value>
        public DateTime LastAccessTime { get; internal set; }

        /// <summary>
        ///   Gets the total count of access to this resource (include incrementing / decrementing its reference count).
        /// </summary>
        /// <value>The access total count.</value>
        public long AccessTotalCount { get; internal set; }

        /// <summary>
        ///   Gets the access count since the last recycle policy was run.
        /// </summary>
        /// <value>The access count since last recycle.</value>
        public long AccessCountSinceLastRecycle { get; internal set; }

        /// <summary>
        ///   Gets the number of active references to this resource.
        /// </summary>
        public int ReferenceCount { get; internal set; }
    }
}
