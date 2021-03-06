﻿// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;
using Stride.Engine;

namespace Stride.Rendering.Compositing
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