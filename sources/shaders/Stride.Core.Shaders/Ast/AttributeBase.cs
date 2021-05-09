// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Shaders.Ast
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
