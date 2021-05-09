// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_DIRECT3D11

using System;
using System.Collections.Generic;

using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Stride.Graphics
{
    /// <summary>
    /// Features supported by a <see cref="GraphicsDevice"/>.
    /// </summary>
    /// <remarks>
    /// This class gives also features for a particular format, using the operator this[dxgiFormat] on this structure.
    /// </remarks>
    public partial struct GraphicsDeviceFeatures
    {
        private static readonly List<SharpDX.DXGI.Format> ObsoleteFormatToExcludes = new List<SharpDX.DXGI.Format>() { Format.R1_UNorm, Format.B5G6R5_UNorm, Format.B5G5R5A1_UNorm };

        internal GraphicsDeviceFeatures(GraphicsDevice deviceRoot)
        {
            var nativeDevice = deviceRoot.NativeDevice;

            HasSRgb = true;

            mapFeaturesPerFormat = new FeaturesPerFormat[256];

            // Set back the real GraphicsProfile that is used
            RequestedProfile = deviceRoot.RequestedProfile;
            CurrentProfile = GraphicsProfileHelper.FromFeatureLevel(nativeDevice.FeatureLevel);

            HasResourceRenaming = true;

            HasComputeShaders = nativeDevice.CheckFeatureSupport(SharpDX.Direct3D11.Feature.ComputeShaders);
            HasDoublePrecision = nativeDevice.CheckFeatureSupport(SharpDX.Direct3D11.Feature.ShaderDoubles);
            nativeDevice.CheckThreadingSupport(out HasMultiThreadingConcurrentResources, out this.HasDriverCommandLists);

            HasDepthAsSRV = true;
            HasDepthAsReadOnlyRT = true;
            HasMultisampleDepthAsSRV = true;

            // Check features for each DXGI.Format
            foreach (var format in Enum.GetValues(typeof(SharpDX.DXGI.Format)))
            {
                var dxgiFormat = (SharpDX.DXGI.Format)format;
                var maximumMultisampleCount = MultisampleCount.None;
                var computeShaderFormatSupport = ComputeShaderFormatSupport.None;
                var formatSupport = FormatSupport.None;

                if (!ObsoleteFormatToExcludes.Contains(dxgiFormat))
                {
                    maximumMultisampleCount = GetMaximumMultisampleCount(nativeDevice, dxgiFormat);
                    if (HasComputeShaders)
                        computeShaderFormatSupport = nativeDevice.CheckComputeShaderFormatSupport(dxgiFormat);

                    formatSupport = (FormatSupport)nativeDevice.CheckFormatSupport(dxgiFormat);
                }

                //mapFeaturesPerFormat[(int)dxgiFormat] = new FeaturesPerFormat((PixelFormat)dxgiFormat, maximumMultisampleCount, computeShaderFormatSupport, formatSupport);
                mapFeaturesPerFormat[(int)dxgiFormat] = new FeaturesPerFormat((PixelFormat)dxgiFormat, maximumMultisampleCount, formatSupport);
            }
        }

        /// <summary>
        /// Gets the maximum multisample count for a particular <see cref="PixelFormat" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="pixelFormat">The pixelFormat.</param>
        /// <returns>The maximum multisample count for this pixel pixelFormat</returns>
        private static MultisampleCount GetMaximumMultisampleCount(SharpDX.Direct3D11.Device device, SharpDX.DXGI.Format pixelFormat)
        {
            int maxCount = 1;
            for (int i = 1; i <= 8; i *= 2)
            {
                if (device.CheckMultisampleQualityLevels(pixelFormat, i) != 0)
                    maxCount = i;
            }
            return (MultisampleCount)maxCount;
        }
    }
}
#endif
