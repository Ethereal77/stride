// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Shaders.Ast.Hlsl;

namespace Stride.Core.Shaders.Ast.Stride
{
    public partial class StrideConstantBufferType : ConstantBufferType
    {
        /// <summary>
        ///   Resource group keyword (rgroup).
        /// </summary>
        public static readonly StrideConstantBufferType ResourceGroup = new StrideConstantBufferType("rgroup");

        /// <summary>
        /// Initializes a new instance of the <see cref="StrideStorageQualifier"/> class.
        /// </summary>
        public StrideConstantBufferType()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="StrideStorageQualifier"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public StrideConstantBufferType(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Parses the specified enum name.
        /// </summary>
        /// <param name="enumName">
        /// Name of the enum.
        /// </param>
        /// <returns>
        /// A qualifier
        /// </returns>
        public static new ConstantBufferType Parse(string enumName)
        {
            if (enumName == (string)ResourceGroup.Key)
                return ResourceGroup;

            return ConstantBufferType.Parse(enumName);
        }
    }
}
