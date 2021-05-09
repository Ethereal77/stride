// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Shaders.Ast
{
    /// <summary>
    /// An interface used by generic definitions and instance.
    /// </summary>
    public interface IGenerics
    {
        /// <summary>
        /// Gets or sets the generic arguments.
        /// </summary>
        /// <value>
        /// The generic arguments.
        /// </value>
        List<TypeBase> GenericParameters { get; set; }

        /// <inheritdoc/>
        List<TypeBase> GenericArguments { get; set; }
    }
}
