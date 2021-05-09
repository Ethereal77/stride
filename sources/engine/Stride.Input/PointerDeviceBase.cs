// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;

using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    ///   Represents the base class for pointer devices.
    /// </summary>
    public abstract class PointerDeviceBase : IPointerDevice
    {
        protected PointerDeviceState PointerState;

        protected PointerDeviceBase()
        {
            PointerState = new PointerDeviceState(this);
        }

        public Vector2 SurfaceSize => PointerState.SurfaceSize;
        public float SurfaceAspectRatio => PointerState.SurfaceAspectRatio;
        public Core.Collections.IReadOnlySet<PointerPoint> PressedPointers => PointerState.PressedPointers;
        public Core.Collections.IReadOnlySet<PointerPoint> ReleasedPointers => PointerState.ReleasedPointers;
        public Core.Collections.IReadOnlySet<PointerPoint> DownPointers => PointerState.DownPointers;
        public event EventHandler<SurfaceSizeChangedEventArgs> SurfaceSizeChanged;

        public int Priority { get; set; }

        public abstract string Name { get; }
        public abstract Guid Id { get; }
        public abstract IInputSource Source { get; }

        public virtual void Update(List<InputEvent> inputEvents)
        {
            PointerState.Update(inputEvents);
        }

        /// <summary>
        ///   Calls <see cref="PointerDeviceState.SetSurfaceSize"/> and invokes the <see cref="SurfaceSizeChanged"/> event.
        /// </summary>
        /// <param name="newSize">New size of the surface.</param>
        protected void SetSurfaceSize(Vector2 newSize)
        {
            PointerState.SetSurfaceSize(newSize);
            SurfaceSizeChanged?.Invoke(this, new SurfaceSizeChangedEventArgs { NewSurfaceSize = newSize });
        }

        protected Vector2 Normalize(Vector2 position)
        {
            return position * PointerState.InverseSurfaceSize;
        }
    }
}
