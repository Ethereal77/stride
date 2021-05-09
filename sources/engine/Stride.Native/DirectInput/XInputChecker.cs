// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

using Stride.Core;

namespace Stride.Native.DirectInput
{
    /// <summary>
    /// Finds out if a device is an XInputDevice
    /// </summary>
    public static class XInputChecker
    {
        static XInputChecker()
        {
            NativeInvoke.PreLoad();
        }

        /// <summary>
        /// Check if device represented by <paramref name="guid"/> is indeed an XInput device.
        /// </summary>
        /// <param name="guid">Guid of device to check.</param>
        /// <returns>True if XInput device.</returns>
        [DllImport(NativeInvoke.Library, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsXInputDevice(ref Guid guid);
    }
}
