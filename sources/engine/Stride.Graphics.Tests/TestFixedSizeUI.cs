// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Games;
using Stride.Rendering.Compositing;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;

using Xunit;

namespace Stride.Graphics.Tests
{
    public class TestFixedSizeUI : GraphicTestGameBase
    {

        private SpriteFont arial16;


        public TestFixedSizeUI() { }

        protected override void RegisterTests()
        {
            base.RegisterTests();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //var time = (float)gameTime.Total.TotalSeconds;
            //cubeEntity.Transform.Rotation = Quaternion.RotationY(time) * Quaternion.RotationX(time * 0.5f);
        }

        protected  Entity GetUIEntity(SpriteFont font, bool fixedSize, Vector3 position)
        {
            // Create and initialize "Touch Screen to Start"
            var touchStartLabel = new ContentDecorator
            {
                Content = new TextBlock
                {
                    Font = font,
                    TextSize = 32,
                    Text = (fixedSize) ? "Fixed Size UI" : "Regular UI",
                    TextColor = Color.White
                },
                Padding = new Thickness(30, 20, 30, 25),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            //touchStartLabel.SetPanelZIndex(1);

            var grid = new Grid
            {
                BackgroundColor = (fixedSize) ? new Color(255, 0, 255) : new Color(255, 255, 0),
                MaximumWidth = 100,
                MaximumHeight = 100,
                MinimumWidth = 100,
                MinimumHeight = 100,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            grid.RowDefinitions.Add(new StripDefinition(StripType.Auto));
            grid.ColumnDefinitions.Add(new StripDefinition());
            grid.LayerDefinitions.Add(new StripDefinition());

            grid.Children.Add(touchStartLabel);

            // Add the background
            var background = new ImageElement { StretchType = StretchType.Fill };
            background.SetPanelZIndex(-1);

            var uiEntity = new Entity();

            // Create a procedural model with a diffuse material
            var uiComponent = new UIComponent
            {
                Page = new UIPage
                {
                    RootElement = new UniformGrid { Children = { background, grid } }
                },
                //IsBillboard = true,
                IsFixedSize = fixedSize,
                IsFullScreen = false,
                Resolution = new Vector3(100, 100, 100), // Same size as the inner grid
                Size = new Vector3(0.1f), // 10% of the vertical resolution
            };
            uiEntity.Add(uiComponent);

            uiEntity.Transform.Position = position;

            return uiEntity;
        }


        protected override async Task LoadContent()
        {
            await base.LoadContent();

            Window.AllowUserResizing = true;

            await base.LoadContent();

            Window.AllowUserResizing = true;

            arial16 = Content.Load<SpriteFont>("DynamicFonts/Arial16");

            // Instantiate a scene with a single entity and model component
            var scene = new Scene();

            // Fixed size
            scene.Entities.Add(GetUIEntity(arial16, true, new Vector3(0, 0, 4)));
            scene.Entities.Add(GetUIEntity(arial16, true, new Vector3(-2, 1, 0)));
            scene.Entities.Add(GetUIEntity(arial16, true, new Vector3(2, 2, -2)));
            scene.Entities.Add(GetUIEntity(arial16, true, new Vector3(0, 1, 0)));
            scene.Entities.Add(GetUIEntity(arial16, true, new Vector3(0, 2, -2)));

            scene.Entities.Add(GetUIEntity(arial16, false, new Vector3(0, -0.3f, 4)));
            scene.Entities.Add(GetUIEntity(arial16, false, new Vector3(-2, 0, 0)));
            scene.Entities.Add(GetUIEntity(arial16, false, new Vector3(2, 1, -2)));

            // Use this graphics compositor
            SceneSystem.GraphicsCompositor = GraphicsCompositorHelper.CreateDefault(false, graphicsProfile: GraphicsProfile.Level_11_0);

            // Create a camera entity and add it to the scene
            var cameraEntity = new Entity { new CameraComponent { Slot = SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId() } };
            cameraEntity.Transform.Position = new Vector3(0, 0, 5);
            scene.Entities.Add(cameraEntity);


            // Create a scene instance
            SceneSystem.SceneInstance = new SceneInstance(Services, scene);
        }

        /// <summary>
        /// Run the test
        /// </summary>
        [Fact]
        public void RunFixedSizeUI()
        {
            RunGameTest(new TestFixedSizeUI());
        }
    }
}
