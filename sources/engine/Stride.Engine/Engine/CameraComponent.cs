// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.ComponentModel;

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Core.Reflection;
using Stride.Engine.Design;
using Stride.Engine.Processors;
using Stride.Rendering.Compositing;

namespace Stride.Engine
{
    /// <summary>
    ///   Represents a camera through which to render the scene and its projection and view parameters.
    /// </summary>
    [DataContract("CameraComponent")]
    [Display("Camera", Expand = ExpandRule.Once)]
    //[DefaultEntityComponentRenderer(typeof(CameraComponentRenderer), -1000)]
    [DefaultEntityComponentProcessor(typeof(CameraProcessor))]
    [ComponentOrder(13000)]
    [ObjectFactory(typeof(CameraComponent.Factory))]
    public sealed class CameraComponent : ActivableEntityComponent
    {
        public const float DefaultAspectRatio = 16.0f / 9.0f;

        public const float DefaultOrthographicSize = 10.0f;

        public const float DefaultVerticalFieldOfView = 45.0f;

        public const float DefaultNearClipPlane = 0.1f;
        public const float DefaultFarClipPlane = 1000.0f;

        /// <summary>
        ///   Initializes a new instance of the <see cref="CameraComponent"/> class.
        /// </summary>
        public CameraComponent() : this(DefaultNearClipPlane, DefaultFarClipPlane) { }

        /// <summary>
        ///   Initializes a new instance of the <see cref="CameraComponent" /> class with the provided
        ///   near and far clipping planes.
        /// </summary>
        /// <param name="nearClipPlane">The near clipping plane.</param>
        /// <param name="farClipPlane">The far clipping plane.</param>
        public CameraComponent(float nearClipPlane, float farClipPlane)
        {
            Projection = CameraProjectionMode.Perspective;
            VerticalFieldOfView = DefaultVerticalFieldOfView;
            OrthographicSize = DefaultOrthographicSize;
            AspectRatio = DefaultAspectRatio;
            NearClipPlane = nearClipPlane;
            FarClipPlane = farClipPlane;
        }

        [DataMember(-5)]
        [Obsolete("This property is no longer used.")]
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the projection mode of the camera.
        /// </summary>
        /// <value>The projection type.</value>
        /// <userdoc>The type of projection used by the camera.</userdoc>
        [DataMember(0)]
        [NotNull]
        public CameraProjectionMode Projection { get; set; }

        /// <summary>
        ///   Gets or sets the vertical field of view, in degrees.
        /// </summary>
        /// <value>
        ///   The vertical field of view.
        /// </value>
        /// <userdoc>The vertical field of view (in degrees).</userdoc>
        [DataMember(5)]
        [DefaultValue(DefaultVerticalFieldOfView)]
        [Display("Field of view")]
        [DataMemberRange(1.0, 179.0, 1.0, 10.0, 0)]
        public float VerticalFieldOfView { get; set; }

        /// <summary>
        ///   Gets or sets the height of the orthographic projection.
        /// </summary>
        /// <value>
        ///   The height of the orthographic projection.
        /// </value>
        /// <remarks>
        ///   the orthographic width is automatically calculated based on the aspect ratio.
        /// </remarks>
        /// <userdoc>The height of the orthographic projection (the orthographic width is automatically calculated based on the target ratio).</userdoc>
        [DataMember(10)]
        [DefaultValue(DefaultOrthographicSize)]
        [Display("Orthographic size")]
        public float OrthographicSize { get; set; }

        /// <summary>
        ///   Gets or sets the near clipping plane distance.
        /// </summary>
        /// <value>
        ///   The near clipping plane distance.
        /// </value>
        /// <userdoc>The distance of the nearest point the camera can see.</userdoc>
        [DataMember(20)]
        [DefaultValue(DefaultNearClipPlane)]
        public float NearClipPlane { get; set; }

        /// <summary>
        ///   Gets or sets the far clipping plane distance.
        /// </summary>
        /// <value>
        ///   The far plane clipping distance.
        /// </value>
        /// <userdoc>The distance of the furthest point the camera can see.</userdoc>
        [DataMember(30)]
        [DefaultValue(DefaultFarClipPlane)]
        public float FarClipPlane { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use a custom <see cref="AspectRatio"/>.
        /// </summary>
        /// <value>
        ///   A value indicating whether to use a custom aspect ratio. The default value is <c>false</c>, meaning
        ///   that the aspect ratio is calculated from the ratio of the current viewport when rendering.
        /// </value>
        /// <userdoc>Use a custom aspect ratio you specify. Otherwise, automatically adjust the aspect ratio to the render target ratio.</userdoc>
        [DataMember(35)]
        [DefaultValue(false)]
        [Display("Custom aspect ratio")]
        public bool UseCustomAspectRatio { get; set; }

        /// <summary>
        ///   Gets or sets the custom aspect ratio.
        /// </summary>
        /// <value>
        ///   The custom aspect ratio.
        /// </value>
        /// <remarks>
        ///   The aspect ratio is the relation between the width and the height of the rendering viewport,
        ///   <c>viewportWidth / viewportHeight</c>.
        /// </remarks>
        /// <userdoc>The aspect ratio for the camera (when using the 'Custom aspect ratio' option).</userdoc>
        [DataMember(40)]
        [DefaultValue(DefaultAspectRatio)]
        public float AspectRatio { get; set; }  // TODO: This should be called CustomAspectRatio

        /// <summary>
        ///   Gets the last used aspect ratio.
        /// </summary>
        /// <remarks>
        ///   This value is updated when calling <see cref="Update"/>.
        ///   It either holds the aspect ratio of the window that called <c>Update</c> or the manually set
        ///   <see cref="AspectRatio"/> if <see cref="UseCustomAspectRatio"/> is <c>true</c>.
        /// </remarks>
        [DataMemberIgnore]
        public float ActuallyUsedAspectRatio { get; private set; } = 1; // TODO: This should be called AspectRatio

        /// <userdoc>The camera slot used in the graphics compositor.</userdoc>
        [DataMember(50)]
        public SceneCameraSlotId Slot;

        /// <summary>
        ///   Gets or sets a value indicating whether to use a custom <see cref="ViewMatrix"/>.
        /// </summary>
        /// <value>
        ///   <c>true</c> to use a custom <see cref="ViewMatrix"/>; otherwise, <c>false</c>.
        ///   Default is <c>false</c>.
        /// </value>
        [DataMemberIgnore]
        public bool UseCustomViewMatrix { get; set; }

        /// <summary>
        ///   Gets or sets the local view matrix.
        /// </summary>
        /// <value>The local view matrix.</value>
        /// <remarks>
        ///   This value is updated when calling <see cref="Update"/> or is directly used when <see cref="UseCustomViewMatrix"/>
        ///   is <c>true</c>.
        /// </remarks>
        [DataMemberIgnore]
        public Matrix ViewMatrix;

        /// <summary>
        ///   Gets or sets a value indicating whether to use a custom <see cref="ProjectionMatrix"/>.
        /// </summary>
        /// <value>
        ///   <c>true</c> to use a custom <see cref="ProjectionMatrix"/>; otherwise, <c>false</c>.
        ///   Default is <c>false</c>
        /// </value>
        [DataMemberIgnore]
        public bool UseCustomProjectionMatrix { get; set; }

        /// <summary>
        ///   Gets or sets the local projection matrix.
        /// </summary>
        /// <value>The local projection matrix.</value>
        /// <remarks>
        ///   This value is updated when calling <see cref="Update"/> or is directly used when <see cref="UseCustomViewMatrix"/>
        ///   is <c>true</c>.
        /// </remarks>
        [DataMemberIgnore]
        public Matrix ProjectionMatrix;

        /// <summary>
        ///   The view projection matrix calculated automatically after calling the <see cref="Update"/> method.
        /// </summary>
        [DataMemberIgnore]
        public Matrix ViewProjectionMatrix;

        /// <summary>
        ///   The camera frustum extracted from the view projection matrix.
        /// </summary>
        /// <remarks>
        ///   The camera frustum is a volume representing the space that the camera can see. It is calculated
        ///   automatically after calling the <see cref="Update"/> method.
        /// </remarks>
        [DataMemberIgnore]
        public BoundingFrustum Frustum;

        /// <summary>
        ///   Calculates the projection matrix and view matrix.
        /// </summary>
        public void Update()
        {
            Update(screenAspectRatio: null);
        }

        /// <summary>
        ///   Calculates the projection matrix and view matrix.
        /// </summary>
        /// <param name="screenAspectRatio">
        ///   The current screen aspect ratio. Specify <c>null</c> to use the <see cref="AspectRatio"/> even if
        ///   <see cref="UseCustomAspectRatio"/> is <c>false</c>.
        /// </param>
        public void Update(float? screenAspectRatio)
        {
            // Calculates the aspect ratio. We only set a new aspect ratio when instructed to. Don't fall back on the custom value, if not instructed to.
            // By caching the actually used aspect ratio we are now free to call Update(null) at any time.
            // A helper visualizing the state of the camera will not change it's state.
            if (UseCustomAspectRatio)
                ActuallyUsedAspectRatio = AspectRatio;
            else if (screenAspectRatio.HasValue)
                ActuallyUsedAspectRatio = screenAspectRatio.Value;

            // Calculates the View
            if (!UseCustomViewMatrix)
            {
                var worldMatrix = EnsureEntity.Transform.WorldMatrix;

                worldMatrix.Decompose(out _, out ViewMatrix, out Vector3 translation);

                // Transpose ViewMatrix (rotation only, so equivalent to inversing it)
                ViewMatrix.Transpose();

                // Rotate our translation so that we can inject it in the view matrix directly
                Vector3.TransformCoordinate(ref translation, ref ViewMatrix, out translation);

                // Apply inverse of translation (equivalent to opposite)
                ViewMatrix.TranslationVector = -translation;
            }

            // Calculates the Projection
            // TODO: Should we throw an error if Projection is not set?
            if (!UseCustomProjectionMatrix)
            {
                // Calculates the projection matrix
                ProjectionMatrix = Projection == CameraProjectionMode.Perspective ?
                    Matrix.PerspectiveFovRH(MathUtil.DegreesToRadians(VerticalFieldOfView), ActuallyUsedAspectRatio, NearClipPlane, FarClipPlane) :
                    Matrix.OrthoRH(ActuallyUsedAspectRatio * OrthographicSize, OrthographicSize, NearClipPlane, FarClipPlane);
            }

            // Update ViewProjectionMatrix
            Matrix.Multiply(ref ViewMatrix, ref ProjectionMatrix, out ViewProjectionMatrix);

            // Update the frustum
            Frustum = new BoundingFrustum(ref ViewProjectionMatrix);
        }

        private class Factory : IObjectFactory
        {
            public object New(Type type)
            {
                return new CameraComponent
                {
                    // Disabled by default to not override current camera
                    Enabled = false,

                    Projection = CameraProjectionMode.Perspective
                };
            }
        }
    }
}
