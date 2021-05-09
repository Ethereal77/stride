// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Linq;
using System.Threading.Tasks;

using Stride.Engine;
using Stride.Games;
using Stride.Graphics.Regression;

using Xunit;

namespace Stride.Graphics.Tests
{
    public class TestLightShafts : GraphicTestGameBase
    {
        public TestLightShafts()
        {
            // 2 = Fix projection issues
            // 3 = Simplifiy density parameters
            // 4 = Change random jitter position hash

            GraphicsDeviceManager.PreferredGraphicsProfile = new[] { GraphicsProfile.Level_11_0 };
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
        }

        protected override void PrepareContext()
        {
            base.PrepareContext();

            SceneSystem.InitialGraphicsCompositorUrl = "LightShaftsGraphicsCompositor";
            SceneSystem.InitialSceneUrl = "LightShafts";
        }

        protected override async Task LoadContent()
        {
            await base.LoadContent();

            Window.AllowUserResizing = true;

            var cameraEntity = SceneSystem.SceneInstance.First(x => x.Get<CameraComponent>() != null);
            cameraEntity.Add(new FpsTestCamera() {MoveSpeed = 10.0f });
        }

        protected override void RegisterTests()
        {
            base.RegisterTests();
            FrameGameSystem.TakeScreenshot(2);
        }

        /// <summary>
        /// Run the test
        /// </summary>
        [Fact]
        public void RunLightShafts()
        {
            RunGameTest(new TestLightShafts());
        }
    }
}
