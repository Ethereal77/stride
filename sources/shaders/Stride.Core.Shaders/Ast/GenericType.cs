// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core;

namespace Stride.Core.Shaders.Ast
{
    /// <summary>
    /// Custom generic type.
    /// </summary>
    public partial class GenericType : GenericBaseType
    {
        public GenericType()
        {
        }

        public GenericType(string name, int parameterCount) : base(name, parameterCount)
        {
        }

        /// <inheritdoc/>
        [DataMember]
        public override List<Type> ParameterTypes { get; set; }

        /// <inheritdoc/>
        [DataMember]
        public override List<Node> Parameters { get; set; }
    }
}
