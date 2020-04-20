// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core.Mathematics;

namespace Stride.Input
{
    /// <summary>
    /// An event for when the size of a pointer surface changed
    /// </summary>
    public class SurfaceSizeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The new size of the surface
        /// </summary>
        public Vector2 NewSurfaceSize;
    }
}