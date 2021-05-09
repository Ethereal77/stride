// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF

using SharpDX.DirectInput;

namespace Stride.Input
{
    /// <summary>
    /// Provides easy operations on <see cref="DeviceObjectTypeFlags"/>
    /// </summary>
    internal static class DeviceObjectIdExtensions
    {
        public static bool HasFlags(this DeviceObjectId objectId, DeviceObjectTypeFlags flags)
        {
            return ((int) objectId.Flags & (int) flags) == (int) flags;
        }

        public static bool HasAnyFlag(this DeviceObjectId objectId, DeviceObjectTypeFlags flags)
        {
            return ((int) objectId.Flags & (int) flags) != 0;
        }
    }
}

#endif
