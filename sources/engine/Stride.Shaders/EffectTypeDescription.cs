// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;

using Stride.Core;

namespace Stride.Shaders
{
    /// <summary>
    /// Describes a shader parameter type.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{Class}{RowCount}x{ColumnCount} {Name}")]
    public struct EffectTypeDescription
    {
        /// <summary>
        /// The <see cref="EffectParameterClass"/> of this parameter.
        /// </summary>
        public EffectParameterClass Class;

        /// <summary>
        /// The <see cref="EffectParameterType"/> of this parameter.
        /// </summary>
        public EffectParameterType Type;

        /// <summary>
        /// Number of rows for this element.
        /// </summary>
        public int RowCount;

        /// <summary>
        /// Number of columns for this element.
        /// </summary>
        public int ColumnCount;

        /// <summary>
        /// Number of elements for arrays (0 if not an array).
        /// </summary>
        public int Elements;

        /// <summary>
        /// Size of this element (non-aligned).
        /// </summary>
        public int ElementSize;

        /// <summary>
        /// Name of this structure type.
        /// </summary>
        public string Name;

        /// <summary>
        /// Members in the structure.
        /// </summary>
        public EffectTypeMemberDescription[] Members;
    }
}
