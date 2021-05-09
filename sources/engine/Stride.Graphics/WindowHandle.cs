// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Games;

namespace Stride.Graphics
{
    /// <summary>
    ///   A platform-specific handle for managing and referencing a window.
    /// </summary>
    public class WindowHandle
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="WindowHandle"/> class.
        /// </summary>
        /// <param name="context">The context type the window is created in.</param>
        /// <param name="nativeWindow">The native window instance (WinForms, WPF, etc).</param>
        /// <param name="handle">The associated handle of <paramref name="nativeWindow"/>.</param>
        public WindowHandle(AppContextType context, object nativeWindow, IntPtr handle)
        {
            Context = context;
            NativeWindow = nativeWindow;
            Handle = handle;
        }

        /// <summary>
        ///   The context type the window is created in.
        /// </summary>
        public readonly AppContextType Context;

        /// <summary>
        ///   Gets the native window as an opaque <see cref="System.Object"/>.
        /// </summary>
        public object NativeWindow { get; }

        /// <summary>
        ///   Gets the associated platform-specific handle of <seealso cref="NativeWindow"/>.
        /// </summary>
        public IntPtr Handle { get; }
    }
}
