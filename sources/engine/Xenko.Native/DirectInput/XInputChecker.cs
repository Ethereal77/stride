// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.InteropServices;

using Xenko.Core;

namespace Xenko.Native.DirectInput
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
