// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

using Stride.Core.Mathematics;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using Stride.Graphics.Regression;
using Stride.Rendering.Colors;
using Stride.Rendering.Lights;

namespace Stride.Engine.Tests
{
    /// <summary>
    /// Base class for engine tests.
    /// </summary>
    public class EngineTestBase : GameTestBase
    {
        protected Scene Scene;
        protected Entity Camera;
        protected LightComponent AmbientLight;

        protected CameraComponent CameraComponent
        {
            get {  return Camera.Get<CameraComponent>(); }
            set
            {
                bool alreadyAdded = false;
                for (int i = 0; i < Camera.Components.Count; i++)
                {
                    var component = Camera.Components[i];
                    if (component == value)
                    {
                        alreadyAdded = true;
                        break;
                    }
                    if (component is CameraComponent)
                    {
                        alreadyAdded = true;
                        Camera.Components[i] = value;
                        break;
                    }
                }
                if (!alreadyAdded)
                {
                    Camera.Add(value);
                }
                value.Slot = SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId();
            }
        }

        public EngineTestBase()
        {
        }

        protected override async Task LoadContent()
        {
            await base.LoadContent();

            SceneSystem.GraphicsCompositor = Content.Load<GraphicsCompositor>("GraphicsCompositor");
            Camera = new Entity { new CameraComponent { Slot = SceneSystem.GraphicsCompositor.Cameras[0].ToSlotId() } };

            Scene = new Scene();
            Scene.Entities.Add(Camera);

            AmbientLight = new LightComponent { Type = new LightAmbient { Color = new ColorRgbProvider(Color.White) }, Intensity = 1 };
            var ambientLight = new Entity { AmbientLight };
            Scene.Entities.Add(ambientLight);

            SceneSystem.SceneInstance = new SceneInstance(Services, Scene);
        }
    }
}
