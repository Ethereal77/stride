// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

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
