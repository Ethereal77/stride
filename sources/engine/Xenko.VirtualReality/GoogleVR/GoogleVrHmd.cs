// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if DONT_BUILD_FOR_NOW && XENKO_PLATFORM_ANDROID

using System;
#if XENKO_PLATFORM_ANDROID
using Android.App;
#endif
using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Games;
using Xenko.Graphics;
using Xenko.Rendering;
using Xenko.Rendering.Composers;

namespace Xenko.VirtualReality
{
    internal class GoogleVrHmd : Hmd
    {
        private Matrix headMatrix;

        public GoogleVrHmd(IServiceRegistry registry) : base(registry)
        {
#if XENKO_PLATFORM_ANDROID
            GoogleVr.Startup(Game, (Activity)PlatformAndroid.Context);
#else
            GoogleVr.Startup(Game);
#endif
        }

        public override void Initialize(Entity cameraRoot, CameraComponent leftCamera, CameraComponent rightCamera, bool requireMirror = false)
        {
            var size = RenderFrameSize;
            if (!GoogleVr.Init(size.Width, size.Height))
            {
                throw new Exception("Failed to Init Google VR SDK");
            }

            RenderFrameProvider = new DirectRenderFrameProvider(RenderFrame.FromTexture(
                Texture.New2D(GraphicsDevice, size.Width, size.Height, PixelFormat.R8G8B8A8_UNorm_SRgb, TextureFlags.RenderTarget | TextureFlags.ShaderResource)
            ));

            var compositor = (SceneGraphicsCompositorLayers)Game.SceneSystem.SceneInstance.RootScene.Settings.GraphicsCompositor;
            compositor.Master.Add(new SceneDelegateRenderer((x, y) =>
            {
                var frame = GoogleVr.GetNextFrame();
                GoogleVr.SubmitRenderTarget(x.GraphicsContext, RenderFrameProvider.RenderFrame.RenderTargets[0], frame, 0);
                if (!GoogleVr.SubmitFrame(GraphicsDevice, frame, ref headMatrix))
                {
                    throw new Exception("Failed to SubmitFrame to Google VR SDK");
                }
            }));

            leftCamera.UseCustomProjectionMatrix = true;
            rightCamera.UseCustomProjectionMatrix = true;
            leftCamera.UseCustomViewMatrix = true;
            rightCamera.UseCustomViewMatrix = true;
            leftCamera.NearClipPlane *= ViewScaling;
            rightCamera.NearClipPlane *= ViewScaling;

            if (requireMirror)
            {
                MirrorTexture = RenderFrameProvider.RenderFrame.RenderTargets[0]; //we don't really have a mirror in this case but avoid crashes
            }

            base.Initialize(cameraRoot, leftCamera, rightCamera, requireMirror);
        }

        public override void Draw(GameTime gameTime)
        {
            GoogleVr.GetHeadMatrix(out headMatrix);
            var headMatrixTr = headMatrix;
            headMatrixTr.Transpose();

            Matrix leftEyeMatrix, rightEyeMatrix;
            GoogleVr.GetEyeMatrix(0, out leftEyeMatrix);
            GoogleVr.GetEyeMatrix(1, out rightEyeMatrix);

            Matrix projLeft, projRight;
            GoogleVr.GetPerspectiveMatrix(0, LeftCameraComponent.NearClipPlane, LeftCameraComponent.FarClipPlane, out projLeft);
            GoogleVr.GetPerspectiveMatrix(1, LeftCameraComponent.NearClipPlane, LeftCameraComponent.FarClipPlane, out projRight);

            Vector3 scale, camPos;
            Quaternion camRot;

            //have to make sure it's updated now
            CameraRootEntity.Transform.UpdateWorldMatrix();
            CameraRootEntity.Transform.WorldMatrix.Decompose(out scale, out camRot, out camPos);

            LeftCameraComponent.ProjectionMatrix = projLeft;

            var pos = camPos + leftEyeMatrix.TranslationVector * ViewScaling;
            var rotV = headMatrixTr * Matrix.Scaling(ViewScaling) * Matrix.RotationQuaternion(camRot);
            var finalUp = Vector3.TransformCoordinate(new Vector3(0, 1, 0), rotV);
            var finalForward = Vector3.TransformCoordinate(new Vector3(0, 0, -1), rotV);
            var view = Matrix.LookAtRH(pos, pos + finalForward, finalUp);
            LeftCameraComponent.ViewMatrix = view;

            RightCameraComponent.ProjectionMatrix = projRight;

            pos = camPos + rightEyeMatrix.TranslationVector * ViewScaling;
            rotV = headMatrixTr * Matrix.Scaling(ViewScaling) * Matrix.RotationQuaternion(camRot);
            finalUp = Vector3.TransformCoordinate(new Vector3(0, 1, 0), rotV);
            finalForward = Vector3.TransformCoordinate(new Vector3(0, 0, -1), rotV);
            view = Matrix.LookAtRH(pos, pos + finalForward, finalUp);
            RightCameraComponent.ViewMatrix = view;

            base.Draw(gameTime);
        }

        public override Size2 OptimalRenderFrameSize
        {
            get
            {
                int width, height;
                GoogleVr.GetMaxRenderSize(out width, out height);
                return new Size2(width, height);
            }
        }

        public override DirectRenderFrameProvider RenderFrameProvider { get; protected set; }

        public override Texture MirrorTexture { get; protected set; }

        public override float RenderFrameScaling { get; set; } = 0.5f;

        public override DeviceState State => DeviceState.Valid;

        public override bool CanInitialize => true;
    }
}

#endif
