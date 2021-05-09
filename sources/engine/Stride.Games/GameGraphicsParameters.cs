// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using Stride.Graphics;

namespace Stride.Games
{
    /// <summary>
    ///   Describess how data will be displayed to the screen.
    /// </summary>
    public class GameGraphicsParameters
    {
        /// <summary>
        ///   A value that describes the resolution width.
        /// </summary>
        public int PreferredBackBufferWidth;

        /// <summary>
        ///   A value that describes the resolution height.
        /// </summary>
        public int PreferredBackBufferHeight;

        /// <summary>
        ///   A <strong><see cref="SharpDX.DXGI.Format" /></strong> structure describing the display format.
        /// </summary>
        public PixelFormat PreferredBackBufferFormat;

        /// <summary>
        /// Gets or sets the depth stencil format
        /// </summary>
        public PixelFormat PreferredDepthStencilFormat;

        /// <summary>
        ///   Gets or sets a value indicating whether the application is in full screen mode.
        /// </summary>
        public bool IsFullScreen;

        /// <summary>
        /// The output (monitor) index to use when switching to fullscreen mode. Doesn't have any effect when windowed mode is used.
        /// </summary>
        public int PreferredFullScreenOutputIndex;

        /// <summary>
        /// Gets or sets the minimum graphics profile.
        /// </summary>
        public GraphicsProfile[] PreferredGraphicsProfile;

        /// <summary>
        /// The preferred refresh rate
        /// </summary>
        public Rational PreferredRefreshRate;

        /// <summary>
        ///   Gets or sets a value indicating the number of sample locations during multisampling.
        /// </summary>
        public MultisampleCount PreferredMultisampleCount;

        /// <summary>
        /// Gets or sets a value indicating whether to synochrnize present with vertical blanking.
        /// </summary>
        public bool SynchronizeWithVerticalRetrace;

        /// <summary>
        /// Gets or sets the colorspace.
        /// </summary>
        public ColorSpace ColorSpace;

        /// <summary>
        /// If populated the engine will try to initialize the device with the same unique id
        /// </summary>
        public string RequiredAdapterUid;
    }
}
