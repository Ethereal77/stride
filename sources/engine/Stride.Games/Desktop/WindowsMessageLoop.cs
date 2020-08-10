// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if STRIDE_UI_WINFORMS || STRIDE_UI_WPF

using System;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;

#if !STRIDE_GRAPHICS_API_NULL
using SharpDX.Win32;
#endif

namespace Stride.Games
{
    /// <summary>
    ///   Represents a class that provides a message loop infrastructure for Windows platforms.
    /// </summary>
    /// <remarks>
    ///   You can use the <see cref="Run(Control, RenderCallback, bool)"/> method to directly run a render loop with
    ///   a callback to render.
    ///   <para/>
    ///   You can also setup your own loop:
    ///   <code>
    ///     control.Show();
    ///     using (var loop = new WindowsMessageLoop(control))
    ///     {
    ///         while (loop.NextFrame())
    ///         {
    ///            /* Perform draw operations here */
    ///         }
    ///     }
    ///   </code>
    ///   Note that the main control can be changed anytime inside the loop.
    /// </remarks>
    internal class WindowsMessageLoop : IMessageLoop
    {
        private IntPtr controlHandle;
        private Control control;
        private bool isControlAlive;
        private bool switchControl;


        /// <summary>
        ///   Initializes a new instance of the <see cref="WindowsMessageLoop"/> class.
        /// </summary>
        public WindowsMessageLoop() { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WindowsMessageLoop"/> class.
        /// </summary>
        /// <param name="control">The control to use for the message loop.</param>
        public WindowsMessageLoop(Control control)
        {
            Control = control;
        }


        /// <summary>
        ///   Gets or sets the control to associate with the current message loop.
        /// </summary>
        /// <value>The control.</value>
        /// <exception cref="System.InvalidOperationException">The control is already disposed.</exception>
        public Control Control
        {
            get => control;

            set
            {
                if (control == value)
                    return;

                // Remove any previous control
                if (control != null && !switchControl)
                {
                    isControlAlive = false;
                    control.Disposed -= ControlDisposed;
                    controlHandle = IntPtr.Zero;
                }

                if (value != null && value.IsDisposed)
                    throw new InvalidOperationException("The control is already disposed.");

                control = value;
                switchControl = true;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the message loop should use the default <see cref="Application.DoEvents"/> instead of
        ///   a custom window message loop.
        /// </summary>
        /// <value>
        ///   <c>true</c> to use the default <see cref="Application.DoEvents"/> instead of a custom window message loop;
        ///   otherwise, <c>false</c>.
        ///   Default value is <c>false</c>.
        /// </value>
        /// <remarks>
        ///   By default, <see cref="WindowsMessageLoop"/> is using a custom window message loop that is more lightweight than the one in
        ///   <see cref="Application.DoEvents" /> to process windows event messages.
        /// </remarks>
        public bool UseApplicationDoEvents { get; set; }

        /// <summary>
        ///   Method called each frame.
        /// </summary>
        /// <returns><c>true</c> if if the control is still active; <c>false</c> otherwise.</returns>
        /// <exception cref="InvalidOperationException">An error occurred </exception>
        public bool NextFrame()
        {
            // Setup the new control
            // TODO: This is not completely thread-safe. We should use a lock to handle this correctly
            if (switchControl && control != null)
            {
                controlHandle = control.Handle;
                control.Disposed += ControlDisposed;
                isControlAlive = true;
                switchControl = false;
            }

            if (isControlAlive)
            {
                if (UseApplicationDoEvents)
                {
                    // Revert back to Application.DoEvents in order to support Application.AddMessageFilter
                    Application.DoEvents();
                }
                else
                {
                    var localHandle = controlHandle;
                    if (localHandle != IntPtr.Zero)
                    {
                        // Previous code not compatible with Application.AddMessageFilter but faster then DoEvents
                        while (Win32Native.PeekMessage(out Win32Native.NativeMessage msg, IntPtr.Zero, 0, 0, Win32Native.PM_REMOVE) != 0)
                        {
                            // NCDESTROY event?
                            if (msg.msg == 130)
                            {
                                isControlAlive = false;
                            }

                            var message = new Message() { HWnd = msg.handle, LParam = msg.lParam, Msg = (int) msg.msg, WParam = msg.wParam };

                            // Skip special message
                            //if (gameForm != null && message.HWnd == gameForm.Handle && gameForm.FilterWindowsKeys(ref message))
                            //{
                            //    continue;
                            //}
                            if (!Application.FilterMessage(ref message))
                            {
                                Win32Native.TranslateMessage(ref msg);
                                Win32Native.DispatchMessage(ref msg);
                            }
                        }
                    }
                }
            }

            return isControlAlive || switchControl;
        }

        private void ControlDisposed(object sender, EventArgs e)
        {
            isControlAlive = false;
        }

        /// <summary>
        ///   Disposes the current <see cref="WindowsMessageLoop"/>.
        /// </summary>
        public void Dispose()
        {
            Control = null;
        }

        /// <summary>
        ///   Delegate representing the function to call for the message loop.
        /// </summary>
        public delegate void RenderCallback();

        /// <summary>
        ///   Runs the specified main loop in the specified context.
        /// </summary>
        /// <param name="context">The application context.</param>
        /// <param name="renderCallback">The function to call every frame.</param>
        public static void Run(ApplicationContext context, RenderCallback renderCallback)
        {
            Run(context.MainForm, renderCallback);
        }

        /// <summary>
        ///   Runs the specified main loop for the specified <see cref="Windows.Forms.Control"/>.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="renderCallback">The function to call every frame.</param>
        /// <param name="useApplicationDoEvents">
        ///   <c>true</c> to use the default <see cref="Application.DoEvents"/> instead of a custom window message loop;
        ///   <c>false</c> otherwise.
        ///   Default is <c>false</c>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="form"/> or <paramref name="renderCallback"/> are a <c>null</c> reference.</exception>
        public static void Run(Control form, RenderCallback renderCallback, bool useApplicationDoEvents = false)
        {
            if (form is null)
                throw new ArgumentNullException(nameof(form));
            if (renderCallback is null)
                throw new ArgumentNullException(nameof(renderCallback));

            form.Show();

            using (var renderLoop = new WindowsMessageLoop(form) { UseApplicationDoEvents = useApplicationDoEvents })
            {
                while (renderLoop.NextFrame())
                {
                    renderCallback();
                }
            }
        }
   }
}
#endif
