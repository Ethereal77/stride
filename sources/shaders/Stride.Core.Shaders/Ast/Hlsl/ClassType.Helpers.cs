// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Shaders.Ast.Hlsl
{
    /// <summary>
    /// Definition of a class.
    /// </summary>
    public partial class ClassType
    {
        /// <summary>
        /// Determines whether the specified type is a a stream type.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns><c>true</c> if [the specified target type] [is stream type] ; otherwise, <c>false</c>.</returns>
        public static bool IsStreamOutputType(TypeBase targetType)
        {
            return targetType is ClassType && ((ClassType)targetType).GenericArguments.Count > 0 && targetType.IsStreamTypeName();
        }
    }
}
