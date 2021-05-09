// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Graphics;

using Half = Stride.Core.Mathematics.Half;

namespace Stride.Rendering.Images
{
    /// <summary>
    ///   Represents an <see cref="ImageEffect"/> that can compute the average luminance of an input texture.
    /// </summary>
    public class LuminanceEffect : ImageEffect
    {
        public static readonly ObjectParameterKey<LuminanceResult> LuminanceResult = ParameterKeys.NewObject<LuminanceResult>();

        private PixelFormat luminanceFormat = PixelFormat.R16_Float;
        private GaussianBlur blur;

        private ImageMultiScaler multiScaler;
        private ImageReadback<Half> readback;

        /// <summary>
        ///   Initializes a new instance of the <see cref="LuminanceEffect" /> class.
        /// </summary>
        public LuminanceEffect()
        {
            LuminanceFormat = PixelFormat.R16_Float;
            DownscaleCount = 6;
            UpscaleCount = 4;
            EnableAverageLuminanceReadback = true;
            readback = new ImageReadback<Half>();
        }

        protected override void InitializeCore()
        {
            base.InitializeCore();

            LuminanceLogEffect = ToLoadAndUnload(new LuminanceLogEffect());

            // Create 1x1 texture
            AverageLuminanceTexture = Texture.New2D(GraphicsDevice, 1, 1, 1, luminanceFormat, TextureFlags.ShaderResource | TextureFlags.RenderTarget).DisposeBy(this);

            // Use a multiscaler
            multiScaler = ToLoadAndUnload(new ImageMultiScaler());

            // Readback is always going to be done on the 1x1 texture
            readback = ToLoadAndUnload(readback);

            // Blur used before upscaling
            blur = ToLoadAndUnload(new GaussianBlur());
            blur.Radius = 4;
        }

        /// <summary>
        ///   Gets or sets the luminance texture format.
        /// </summary>
        public PixelFormat LuminanceFormat
        {
            get => luminanceFormat;

            set
            {
                if (value.IsCompressed() ||
                    value.IsPacked() ||
                    value.IsTypeless() ||
                    value == PixelFormat.None)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"Unsupported format [{value}] (can't be None, Compressed, Packed nor Typeless).");
                }
                luminanceFormat = value;
            }
        }

        /// <summary>
        ///   Gets or sets the effect that computes the logarithmic (log2) luminance from the input texture.
        /// </summary>
        public ImageEffectShader LuminanceLogEffect { get; set; }

        /// <summary>
        ///   Gets or sets the downscale count of the input intermediate texture used for local luminance (if no
        ///   output is given).
        /// </summary>
        /// <value>Down scale count. By default 1/64 of the input texture size.</value>
        public int DownscaleCount { get; set; }

        /// <summary>
        ///   Gets or sets the upscale count of the downscaled input local luminance texture.
        /// </summary>
        /// <value>The upscale count. By default x16 of the input texture size.</value>
        public int UpscaleCount { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to enable the read back of the calculated <see cref="AverageLuminance"/>
        ///   by the CPU.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to enable calculation of <see cref="AverageLuminance"/>; otherwise, <c>false</c>.
        ///   Default is <c>true</c>.
        /// </value>
        public bool EnableAverageLuminanceReadback { get; set; }

        /// <summary>
        ///   Gets the average luminance calculated on the GPU.
        /// </summary>
        /// <value>The average luminance.</value>
        /// <remarks>
        ///   The average luminance is calculated on the GPU and readback with a few frames of delay, depending
        ///   on the number of frames in advance between command scheduling and actual execution on GPU.
        /// </remarks>
        public float AverageLuminance { get; private set; }

        /// <summary>
        ///   Gets the average luminance 1x1 texture available after drawing this effect.
        /// </summary>
        /// <value>The average luminance texture.</value>
        public Texture AverageLuminanceTexture { get; private set; }

        /// <summary>
        ///   Gets or sets a value indicating if the local luminance should be rendered to the output texture.
        /// </summary>
        public bool EnableLocalLuminanceCalculation { get; set; }

        public override void Reset()
        {
            readback.Reset();

            base.Reset();
        }

        protected override void DrawCore(RenderDrawContext context)
        {
            var input = GetSafeInput(0);

            // Render the luminance to a power-of-two target, so we preserve energy on downscaling
            var startWidth = Math.Clamp(MathUtil.NextPowerOfTwo(input.Size.Width), 1, MathUtil.NextPowerOfTwo(input.Size.Height) / 2);
            var startSize = new Size3(startWidth, startWidth, 1);
            var blurTextureSize = startSize.Down2(UpscaleCount);

            // If we don't need a blur pass, or no local luminance output at all, don't allocate a blur target
            Texture blurTexture = null;
            if (EnableLocalLuminanceCalculation && blurTextureSize.Width != 1 && blurTextureSize.Height != 1)
            {
                blurTexture = NewScopedRenderTarget2D(blurTextureSize.Width, blurTextureSize.Height, luminanceFormat, 1);
            }

            var luminanceMap = NewScopedRenderTarget2D(startSize.Width, startSize.Height, luminanceFormat, 1);

            // Calculate the first luminance map
            LuminanceLogEffect.SetInput(input);
            LuminanceLogEffect.SetOutput(luminanceMap);
            LuminanceLogEffect.Draw(context);

            // Downscales luminance up to BlurTexture (optional) and 1x1
            multiScaler.SetInput(luminanceMap);
            if (blurTexture is null)
            {
                multiScaler.SetOutput(AverageLuminanceTexture);
                multiScaler.Draw(context);

                if (EnableLocalLuminanceCalculation)
                {
                    var output = GetSafeOutput(0);

                    // TODO: Workaround to that the output filled with 1x1
                    Scaler.SetInput(AverageLuminanceTexture);
                    Scaler.SetOutput(output);
                    Scaler.Draw(context);
                }
            }
            else
            {
                multiScaler.SetOutput(blurTexture, AverageLuminanceTexture);
                multiScaler.Draw(context);

                // Blur x2 the intermediate output texture
                blur.SetInput(blurTexture);
                blur.SetOutput(blurTexture);
                blur.Draw(context);
                blur.Draw(context);

                var output = GetSafeOutput(0);

                // Upscale from intermediate to output
                multiScaler.SetInput(blurTexture);
                multiScaler.SetOutput(output);
                multiScaler.Draw(context);
            }

            // Calculate average luminance only if needed
            if (EnableAverageLuminanceReadback)
            {
                readback.SetInput(AverageLuminanceTexture);
                readback.Draw(context);
                var rawLogValue = readback.Result[0];

                AverageLuminance = (float) Math.Pow(2.0, rawLogValue);

                // In case AvergaeLuminance go crazy because of half float/infinity precision, some code to save the values here:
                //if (float.IsInfinity(AverageLuminance))
                //{
                //    using (var stream = new FileStream("luminance_input.dds", FileMode.Create, FileAccess.Write))
                //    {
                //        input.Save(stream, ImageFileType.Dds);
                //    }
                //    using (var stream = new FileStream("luminance.dds", FileMode.Create, FileAccess.Write))
                //    {
                //        luminanceMap.Save(stream, ImageFileType.Dds);
                //    }
                //}
            }
        }
    }
}
