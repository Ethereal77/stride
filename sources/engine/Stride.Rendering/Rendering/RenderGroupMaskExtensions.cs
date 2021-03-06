// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace Stride.Rendering
{
    /// <summary>
    /// Extensions for <see cref="RenderGroupMask"/>
    /// </summary>
    public static class RenderGroupMaskExtensions
    {
        /// <summary>
        /// Determines whether the group mask contains the specified group.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <param name="group">The group.</param>
        /// <returns><c>true</c> if the group mask contains the specified group; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this RenderGroupMask mask, RenderGroup group)
        {
            return ((uint)mask & (1 << (int)group)) != 0;
        }
    }
}
