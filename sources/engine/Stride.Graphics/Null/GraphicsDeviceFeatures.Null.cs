// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_NULL

namespace Stride.Graphics
{
    /// <summary>
    /// Features supported by a <see cref="GraphicsDevice"/>.
    /// </summary>
    /// <remarks>This class gives also features for a particular format, using the operator this[Format] on this structure. </remarks>
    public partial struct GraphicsDeviceFeatures
    {
        internal GraphicsDeviceFeatures(GraphicsDevice deviceRoot)
        {
            NullHelper.ToImplement();
            mapFeaturesPerFormat = new FeaturesPerFormat[256];
            for (int i = 0; i < mapFeaturesPerFormat.Length; i++)
                mapFeaturesPerFormat[i] = new FeaturesPerFormat((PixelFormat)i, MultisampleCount.None, FormatSupport.None);
            HasComputeShaders = true;
            HasDepthAsReadOnlyRT = false;
            HasDepthAsSRV = true;
            HasMultisampleDepthAsSRV = false;
            HasDoublePrecision = true;
            HasDriverCommandLists = true;
            HasMultiThreadingConcurrentResources = true;
            HasResourceRenaming = true;
            HasSRgb = true;
            RequestedProfile = GraphicsProfile.Level_11_2;
            CurrentProfile = GraphicsProfile.Level_11_2;
        }
    }
}
#endif
