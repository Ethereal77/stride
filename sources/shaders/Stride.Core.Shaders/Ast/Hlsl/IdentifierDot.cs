// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Shaders.Ast.Hlsl
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
