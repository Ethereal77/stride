// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.Serialization;

namespace Stride.Rendering
{
    /// <summary>
    /// Describes a single transformation node, usually in a <see cref="Model"/> node hierarchy.
    /// </summary>
    [DataContract]
    public struct ModelNodeDefinition
    {
        /// <summary>
        /// The parent node index.
        /// </summary>
        public int ParentIndex;

        /// <summary>
        /// The local transform.
        /// </summary>
        public TransformTRS Transform;

        /// <summary>
        /// The name of this node.
        /// </summary>
        public string Name;

        /// <summary>
        /// The flags of this node.
        /// </summary>
        public ModelNodeFlags Flags;

        public override string ToString()
        {
            return string.Format("Parent: {0} Name: {1}", ParentIndex, Name);
        }
    }
}
