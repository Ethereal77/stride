// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Mathematics;

namespace Stride.Rendering.Images.SphericalHarmonics
{
    public class SphericalHarmonicsRendererEffect : ImageEffectShader
    {
        /// <summary>
        /// Gets or sets the harmonic order to use during the filtering.
        /// </summary>
        public Core.Mathematics.SphericalHarmonics InputSH { get; set; }

        public SphericalHarmonicsRendererEffect()
        {
            EffectName = "SphericalHarmonicsRendererEffect";
        }

        protected override void UpdateParameters()
        {
            base.UpdateParameters();

            if (InputSH != null)
            {
                Parameters.Set(SphericalHarmonicsParameters.HarmonicsOrder, InputSH.Order);
                Parameters.Set(SphericalHarmonicsRendererKeys.SHCoefficients, InputSH.Coefficients);
            }
            else
            {
                Parameters.Set(SphericalHarmonicsParameters.HarmonicsOrder, 1);
                Parameters.Set(SphericalHarmonicsRendererKeys.SHCoefficients, new[] { new Color3() });
            }
        }
    }
}
