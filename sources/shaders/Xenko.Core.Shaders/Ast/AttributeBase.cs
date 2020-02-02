// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Shaders.Ast
{
    /// <summary>
    /// An abstract class for attribute definition.
    /// </summary>
    public abstract partial class AttributeBase : Node
    {
    }

    /// <summary>
    /// An abstract class for a post attribute definition.
    /// </summary>
    public abstract partial class PostAttributeBase : AttributeBase
    {
    }
}
