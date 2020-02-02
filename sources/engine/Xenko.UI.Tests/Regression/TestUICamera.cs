// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Graphics.Regression;
using Xenko.Rendering.Compositing;

namespace Xenko.UI.Tests.Regression
{
    public class TestUICamera : TestCamera
    {
        public TestUICamera(GraphicsCompositor graphicsCompositor)
            : base(graphicsCompositor)
        {
        }

        protected override void SetCamera()
        {
            base.SetCamera();
            Camera.NearClipPlane = 1f;
            Camera.FarClipPlane = 10000f;
        }
    }
}
