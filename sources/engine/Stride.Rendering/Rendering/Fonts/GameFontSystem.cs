// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;
using Stride.Core.IO;
using Stride.Games;
using Stride.Graphics.Font;

namespace Stride.Rendering.Fonts
{
    /// <summary>
    /// The game system in charge of calling <see cref="FontSystem"/>.
    /// </summary>
    public class GameFontSystem : GameSystemBase
    {
        public FontSystem FontSystem { get; private set; }

        public GameFontSystem(IServiceRegistry registry)
            : base(registry)
        {
            Visible = true;
            FontSystem = new FontSystem();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            FontSystem.Draw();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            FontSystem.Load(GraphicsDevice, Services.GetSafeServiceAs<IDatabaseFileProviderService>());
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            FontSystem.Unload();
        }
    }
}
