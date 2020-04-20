// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Shaders.Visitor;

namespace Stride.Core.Shaders.Ast
{
    /// <summary>
    /// Instruct a <see cref="ShaderVisitor"/> to ignore a field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class VisitorIgnoreAttribute : Attribute
    {
    }
}
