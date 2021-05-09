// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#pragma warning disable SA1405 // Debug.Assert must provide message text

#if STRIDE_GRAPHICS_API_DIRECT3D11

using System;
using System.Diagnostics;

using SharpDX;

namespace Stride.Graphics
{
    public abstract partial class GraphicsResourceBase
    {
        private SharpDX.Direct3D11.DeviceChild nativeDeviceChild;

        protected internal SharpDX.Direct3D11.Resource NativeResource { get; private set; }

        private void Initialize() { }

        /// <summary>
        ///   Gets or sets the device child.
        /// </summary>
        /// <value>The device child.</value>
        protected internal SharpDX.Direct3D11.DeviceChild NativeDeviceChild
        {
            get => nativeDeviceChild;

            set
            {
                nativeDeviceChild = value;
                NativeResource = nativeDeviceChild as SharpDX.Direct3D11.Resource;
                // Associate PrivateData to this DeviceResource
                SetDebugName(GraphicsDevice, nativeDeviceChild, Name);
            }
        }

        /// <summary>
        ///   Associates a debug name as private data to the device child. Useful to get the name in debuggers.
        /// </summary>
        internal static void SetDebugName(GraphicsDevice graphicsDevice, SharpDX.Direct3D11.DeviceChild deviceChild, string name)
        {
            if (graphicsDevice.IsDebugMode && deviceChild != null)
            {
                deviceChild.DebugName = name;
            }
        }

        /// <summary>
        ///   Method called when the graphics device has been detected to be internally destroyed.
        /// </summary>
        protected internal virtual void OnDestroyed()
        {
            Destroyed?.Invoke(this, EventArgs.Empty);

            ReleaseComObject(ref nativeDeviceChild);
            NativeResource = null;
        }

        /// <summary>
        ///   Method called when the graphics device has been recreated.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the resource has transitioned to a <see cref="GraphicsResourceLifetimeState.Active"/> state.
        /// </returns>
        protected internal virtual bool OnRecreate()
        {
            return false;
        }

        protected SharpDX.Direct3D11.Device NativeDevice => GraphicsDevice?.NativeDevice;

        /// <summary>
        ///   Determines the CPU access flags for a specific resource usage.
        /// </summary>
        /// <param name="usage">The intended usage for the resource.</param>
        /// <returns>The appropriate access flags.</returns>
        internal static SharpDX.Direct3D11.CpuAccessFlags GetCpuAccessFlagsFromUsage(GraphicsResourceUsage usage)
        {
            switch (usage)
            {
                case GraphicsResourceUsage.Dynamic:
                    return SharpDX.Direct3D11.CpuAccessFlags.Write;

                case GraphicsResourceUsage.Staging:
                    return SharpDX.Direct3D11.CpuAccessFlags.Read | SharpDX.Direct3D11.CpuAccessFlags.Write;
            }
            return SharpDX.Direct3D11.CpuAccessFlags.None;
        }

        internal static void ReleaseComObject<T>(ref T comObject) where T : class
        {
            // We can't put IUnknown as a constraint on the generic as it would break compilation (trying to import SharpDX in projects with InternalVisibleTo)
            if (comObject is IUnknown iUnknownObject)
            {
                var refCountResult = iUnknownObject.Release();
                Debug.Assert(refCountResult >= 0);
                comObject = null;
            }
        }
    }
}

#endif
