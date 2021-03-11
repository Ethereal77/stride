// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    ///   Defines the interface for interacting with pointer devices, like a mouse, a pen, a touch screen, etc.
    /// </summary>
    public interface IPointerDevice : IInputDevice
    {
        /// <summary>
        ///   Gets the size of the surface used by the pointer.
        /// </summary>
        /// <remarks>
        ///   For a mouse this is the size of the window. For a touch device, the size of the touch area, etc.
        /// </remarks>
        Vector2 SurfaceSize { get; }

        /// <summary>
        ///   Gets the aspect ratio of the surface area.
        /// </summary>
        float SurfaceAspectRatio { get; }

        /// <summary>
        ///   Gets the pointers that have been pressed since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<PointerPoint> PressedPointers { get; }

        /// <summary>
        ///   Gets the pointers that have been released since the last frame.
        /// </summary>
        Core.Collections.IReadOnlySet<PointerPoint> ReleasedPointers { get; }

        /// <summary>
        ///   Gets the pointers that are currently down.
        /// </summary>
        Core.Collections.IReadOnlySet<PointerPoint> DownPointers { get; }

        /// <summary>
        ///   Raised when the surface size of this pointer has changed.
        /// </summary>
        event EventHandler<SurfaceSizeChangedEventArgs> SurfaceSizeChanged;
    }
}
