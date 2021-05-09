// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core
{
    /// <summary>
    /// Base interface for all referencable objects.
    /// </summary>
    public interface IReferencable
    {
        /// <summary>
        /// Gets the reference count of this instance.
        /// </summary>
        /// <value>
        /// The reference count.
        /// </value>
        int ReferenceCount { get; }

        /// <summary>
        /// Increments the reference count of this instance.
        /// </summary>
        /// <returns>The method returns the new reference count.</returns>
        int AddReference();

        /// <summary>
        /// Decrements the reference count of this instance.
        /// </summary>
        /// <returns>The method returns the new reference count.</returns>
        /// <remarks>When the reference count is going to 0, the component should release/dispose dependents objects.</remarks>
        int Release();
    }
}
