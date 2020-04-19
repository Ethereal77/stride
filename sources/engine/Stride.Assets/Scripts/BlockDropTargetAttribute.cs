// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Assets.Scripts
{
    /// <summary>
    /// Specifies that this field or property should be set if a compatible object is dropped on its containing <see cref="Block"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BlockDropTargetAttribute : Attribute
    {
    }
}
