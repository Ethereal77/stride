// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Shaders.Ast.Hlsl
{
    /// <summary>
    /// C# namespace or class.
    /// </summary>
    public partial class IdentifierDot : CompositeIdentifier
    {
        /// <inheritdoc/>
        public override string Separator
        {
            get
            {
                return ".";
            }
        }
    }
}
