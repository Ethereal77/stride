// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Rendering.Images
{
    /// <summary>
    ///   Represents a simple depth-based fog post-processing effect.
    /// </summary>
    [DataContract("Fog")]
    public class Fog : ImageEffect
    {
        private readonly ImageEffectShader fogFilter;

        private Texture depthTexture;
        private float zMin, zMax;

        /// <summary>
        ///   Initializes a new instance of the <see cref="Fog"/> class.
        /// </summary>
        public Fog() : this("FogEffect") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fog"/> class.
        /// </summary>
        /// <param name="shaderName">Name of the fog shader.</param>
        public Fog(string shaderName) : base(shaderName)
        {
            if (string.IsNullOrWhiteSpace(shaderName))
                throw new ArgumentNullException(nameof(shaderName));

            fogFilter = new ImageEffectShader(shaderName);
        }

        /// <summary>
        ///   Gets or sets the density of the fog.
        /// </summary>
        [DataMember(10)]
        public float Density { get; set; } = 0.1f;

        /// <summary>
        ///   Gets or sets the color of the fog.
        /// </summary>
        [DataMember(20)]
        public Color3 Color { get; set; } = new Color3(1.0f);

        /// <summary>
        ///   Gets or sets the distance at which the fog starts.
        /// </summary>
        [DataMember(30)]
        public float FogStart { get; set; } = 10.0f;

        /// <summary>
        ///   Gets or sets a value indicating whether to avoid applying fog to the maximum depth
        ///   (the background).
        /// </summary>
        [DataMember(40)]
        public bool SkipBackground { get; set; } = false;

        protected override void InitializeCore()
        {
            base.InitializeCore();

            ToLoadAndUnload(fogFilter);
        }

        /// <summary>
        ///   Provides a color buffer and a depth buffer to apply the fog to.
        /// </summary>
        /// <param name="colorBuffer">A color buffer to process.</param>
        /// <param name="depthBuffer">The depth buffer corresponding to the color buffer provided.</param>
        public void SetColorDepthInput(Texture colorBuffer, Texture depthBuffer, float zMin, float zMax)
        {
            SetInput(0, colorBuffer);

            depthTexture = depthBuffer;
            this.zMin = zMin;
            this.zMax = zMax;
        }

        protected override void SetDefaultParameters()
        {
            Color = new Color3(1.0f);
            Density = 0.1f;

            base.SetDefaultParameters();
        }

        protected override void DrawCore(RenderDrawContext context)
        {
            Texture color = GetInput(0);
            Texture output = GetOutput(0);

            if (color is null || output is null || depthTexture is null)
                return;

            fogFilter.Parameters.Set(FogEffectKeys.FogColor, Color);
            fogFilter.Parameters.Set(FogEffectKeys.Density, Density);
            fogFilter.Parameters.Set(FogEffectKeys.DepthTexture, depthTexture);
            fogFilter.Parameters.Set(FogEffectKeys.zFar, zMax);
            fogFilter.Parameters.Set(FogEffectKeys.zNear, zMin);
            fogFilter.Parameters.Set(FogEffectKeys.FogStart, FogStart);
            fogFilter.Parameters.Set(FogEffectKeys.skipBackground, SkipBackground);

            fogFilter.SetInput(0, color);
            fogFilter.SetOutput(output);

            fogFilter.Draw(context);
        }
    }
}
