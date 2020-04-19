// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Annotations;
using Xenko.Engine;

namespace Xenko.Rendering.Compositing
{
    /// <summary>
    /// A camera renderer that can use an external camera not in the scene.
    /// </summary>
    [NonInstantiable]
    public class SceneExternalCameraRenderer : SceneCameraRenderer
    {
        public CameraComponent ExternalCamera { get; set; }

        /// <summary>
        /// Resolves camera to <see cref="ExternalCamera"/> rather than the default behavior.
        /// </summary>
        protected override CameraComponent ResolveCamera(RenderContext renderContext)
        {
            return ExternalCamera;
        }
    }
}