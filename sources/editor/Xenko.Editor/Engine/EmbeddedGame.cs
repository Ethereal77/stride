// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Diagnostics;
using Xenko.Engine;
using Xenko.Graphics;

namespace Xenko.Editor.Engine
{
    /// <summary>
    /// Represents a Game that is embedded in a external window.
    /// </summary>
    public class EmbeddedGame : Game
    {
        /// <summary>
        /// All created embedded games (preview, scene, etc...) will have <see cref="DeviceCreationFlags.Debug"/> set.
        /// </summary>
        public static bool DebugMode { get; set; }

        public EmbeddedGame()
        {
            GraphicsDeviceManager.PreferredGraphicsProfile = new [] { GraphicsProfile.Level_11_2, GraphicsProfile.Level_11_1, GraphicsProfile.Level_11_0 };
            GraphicsDeviceManager.PreferredBackBufferWidth = 64;
            GraphicsDeviceManager.PreferredBackBufferHeight = 64;
            GraphicsDeviceManager.PreferredDepthStencilFormat = PixelFormat.D24_UNorm_S8_UInt;
            GraphicsDeviceManager.DeviceCreationFlags = DebugMode ? DeviceCreationFlags.Debug : DeviceCreationFlags.None;

            AutoLoadDefaultSettings = false;
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            Window.IsBorderLess = true;
            Window.IsMouseVisible = true;
        }

        /// <inheritdoc />
        protected sealed override LogListener GetLogListener()
        {
            // We don't want the embedded games to log in the console
            return null;
        }
    }
}
