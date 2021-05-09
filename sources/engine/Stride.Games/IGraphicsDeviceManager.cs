// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using Stride.Graphics;

namespace Stride.Games
{
    /// <summary>
    ///   Defines the interface for an object that manages a <see cref="GraphicsDevice"/>.
    /// </summary>
    public interface IGraphicsDeviceManager
    {
        /// <summary>
        ///   Starts the drawing of a frame.
        /// </summary>
        /// <returns><c>true</c> if the device is ready and drawing operations can start; <c>false</c> otherwise</returns>
        bool BeginDraw();

        /// <summary>
        ///   Called to ensure that the device manager has created a valid device.
        /// </summary>
        void CreateDevice();

        /// <summary>
        ///   Ends the drawing of the frame, optionally presenting to the screen.
        /// </summary>
        /// <param name="present">A value indicating whether to present the frame.</param>
        void EndDraw(bool present);
    }
}
