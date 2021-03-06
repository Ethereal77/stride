// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_DIRECT3D12

using System;
using System.Collections.Generic;
using System.Diagnostics;

using SharpDX;

namespace Stride.Graphics
{
    public abstract partial class GraphicsResourceBase
    {
        private SharpDX.Direct3D12.DeviceChild nativeDeviceChild;

        protected internal SharpDX.Direct3D12.Resource NativeResource { get; private set; }

        private void Initialize() { }

        /// <summary>
        ///   Gets or sets the device child.
        /// </summary>
        /// <value>The device child.</value>
        protected internal SharpDX.Direct3D12.DeviceChild NativeDeviceChild
        {
            get => nativeDeviceChild;

            set
            {
                nativeDeviceChild = value;
                NativeResource = nativeDeviceChild as SharpDX.Direct3D12.Resource;
                // Associate PrivateData to this DeviceResource
                SetDebugName(GraphicsDevice, nativeDeviceChild, Name);
            }
        }

        /// <summary>
        ///   Associates a debug name as private data to the device child. Useful to get the name in debuggers.
        /// </summary>
        internal static void SetDebugName(GraphicsDevice graphicsDevice, SharpDX.Direct3D12.DeviceChild deviceChild, string name)
        {
            if (graphicsDevice.IsDebugMode && deviceChild != null)
            {
                deviceChild.Name = name;
            }
        }

        /// <summary>
        ///   Method called when the graphics device has been detected to be internally destroyed.
        /// </summary>
        protected internal virtual void OnDestroyed()
        {
            Destroyed?.Invoke(this, EventArgs.Empty);

            if (nativeDeviceChild != null)
            {
                // Schedule the resource for destruction (as soon as we are done with it)
                GraphicsDevice.TemporaryResources.Enqueue(new KeyValuePair<long, object>(GraphicsDevice.NextFenceValue, nativeDeviceChild));
                nativeDeviceChild = null;
            }
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

        protected SharpDX.Direct3D12.Device NativeDevice => GraphicsDevice?.NativeDevice;

        internal static void ReleaseComObject<T>(ref T comObject) where T : class, IUnknown
        {
            // We can't put IUnknown as a constraint on the generic as it would break compilation (trying to import SharpDX in projects with InternalVisibleTo)
            var refCountResult = comObject.Release();
            Debug.Assert(refCountResult >= 0);
            comObject = null;
        }
    }
}

#endif
