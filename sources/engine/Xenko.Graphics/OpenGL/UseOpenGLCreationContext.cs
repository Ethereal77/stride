// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
#if XENKO_GRAPHICS_API_OPENGL
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using OpenTK.Graphics;
#if XENKO_GRAPHICS_API_OPENGLES
using OpenTK.Graphics.ES30;
#else
using OpenTK.Graphics.OpenGL;
#endif

namespace Xenko.Graphics
{
    /// <summary>
    /// Used internally to provide a context for async resource creation
    /// (such as texture or buffer created on a thread where no context is active).
    /// </summary>
    internal struct UseOpenGLCreationContext : IDisposable
    {
        public readonly CommandList CommandList;

        private readonly bool useDeviceCreationContext;
        private readonly bool needUnbindContext;

        private readonly bool asyncCreationLockTaken;
        private readonly object asyncCreationLockObject;

        private readonly IGraphicsContext deviceCreationContext;

        public bool UseDeviceCreationContext => useDeviceCreationContext;

        public UseOpenGLCreationContext(GraphicsDevice graphicsDevice)
            : this()
        {
            if (OpenTK.Graphics.GraphicsContext.CurrentContextHandle.Handle == IntPtr.Zero)
            {
                needUnbindContext = true;
                useDeviceCreationContext = true;

                // Lock, since there is only one deviceCreationContext.
                // TODO: Support multiple deviceCreationContext (TLS creation of context was crashing, need to investigate why)
                asyncCreationLockObject = graphicsDevice.asyncCreationLockObject;
                Monitor.Enter(graphicsDevice.asyncCreationLockObject, ref asyncCreationLockTaken);

                // Bind the context
                deviceCreationContext = graphicsDevice.deviceCreationContext;
                deviceCreationContext.MakeCurrent(graphicsDevice.deviceCreationWindowInfo);
            }
            else
            {
                // TODO Hardcoded to the fact it uses only one command list, this should be fixed
                CommandList = graphicsDevice.InternalMainCommandList;
            }
        }

        public void Dispose()
        {
            try
            {
                if (needUnbindContext)
                {
                    GL.Flush();

                    // Restore graphics context
                    GraphicsDevice.UnbindGraphicsContext(deviceCreationContext);
                }
            }
            finally
            {
                // Unlock
                if (asyncCreationLockTaken)
                {
                    Monitor.Exit(asyncCreationLockObject);
                }
            }
        }
    }
}
#endif
