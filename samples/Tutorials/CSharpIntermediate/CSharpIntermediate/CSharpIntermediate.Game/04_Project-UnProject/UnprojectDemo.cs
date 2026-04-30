// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Graphics;
using Stride.Input;
using Stride.Physics;

namespace CSharpIntermediate.Code
{
    public class UnprojectDemo : SyncScript
    {
        private CameraComponent camera;
        public Entity sphereToClone;

        public override void Start()
        {
            camera = Entity.Get<CameraComponent>();
        }

        public override void Update()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                var backBuffer = GraphicsDevice.Presenter.BackBuffer;
                var viewport = new Viewport(0, 0, backBuffer.Width, backBuffer.Height);

                var nearPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 0.0f), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
                var farPosition = viewport.Unproject(new Vector3(Input.AbsoluteMousePosition, 1.0f), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);

                var hitResult = this.GetSimulation().Raycast(nearPosition, farPosition);

                // If there is a hitresult, clone the sphere and place it on that position
                if (hitResult.Succeeded)
                {
                    var sphereClone = sphereToClone.Clone();
                    sphereClone.Transform.Position = hitResult.Point;
                    Entity.Scene.Entities.Add(sphereClone);
                }
            }
        }
    }
}
