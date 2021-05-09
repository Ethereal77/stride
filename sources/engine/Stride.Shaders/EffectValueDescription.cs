// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Diagnostics;

using Stride.Core;
using Stride.Core.Serialization;

namespace Stride.Shaders
{
    /// <summary>
    ///   Describes a shader parameter for a value type (usually stored in constant buffers).
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{Type.Class}{Type.RowCount}x{Type.ColumnCount} {KeyInfo.KeyName} -> {RawName}")]
    public struct EffectValueDescription
    {
        /// <summary>
        ///   Type of this value.
        /// </summary>
        public EffectTypeDescription Type;

        /// <summary>
        ///   Common description of this parameter.
        /// </summary>
        public EffectParameterKeyInfo KeyInfo;

        /// <summary>
        ///   Name of this parameter in the original shader.
        /// </summary>
        public string RawName;

        /// <summary>
        ///   Offset of this value in the constant buffer, in bytes.
        /// </summary>
        public int Offset;

        /// <summary>
        ///   Size of this value in the constant buffer, in bytes.
        /// </summary>
        public int Size;

        /// <summary>
        ///   Value to use by default.
        /// </summary>
        public object DefaultValue;

        /// <summary>
        ///   Logical group used to group related descriptors and variables together.
        /// </summary>
        public string LogicalGroup;
    }
}
