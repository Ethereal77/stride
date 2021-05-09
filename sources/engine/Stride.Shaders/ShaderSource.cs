// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.IO;

using Stride.Core;
using Stride.Core.Annotations;

namespace Stride.Shaders
{
    /// <summary>
    /// A shader source.
    /// </summary>
    [DataContract("ShaderSource")]
    [NonIdentifiableCollectionItems]
    public abstract class ShaderSource
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ShaderSource"/> is a discard shader after it has been mixed.
        /// </summary>
        /// <value><c>true</c> if discard; otherwise, <c>false</c>.</value>
        [DataMemberIgnore]
        public bool Discard { get; set; }

        /// <summary>
        /// Deep clones this instance.
        /// </summary>
        /// <returns>A new instance.</returns>
        public abstract object Clone();

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="against">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public abstract override bool Equals(object against);

        public abstract override int GetHashCode();
    }
}
