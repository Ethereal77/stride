// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_NULL 

namespace Stride.Graphics
{
    /// <summary>
    /// Base class for texture resources.
    /// </summary>
    public partial class Texture
    {
        /// <summary>
        /// Size of texture in pixel.
        /// </summary>
        private int TexturePixelSize
        {
            get
            {
                NullHelper.ToImplement();
                return 16;
            }
        }

        private const int TextureSubresourceAlignment = 4;
        private const int TextureRowPitchAlignment = 1;

        internal bool HasStencil;

        public void Recreate(DataBox[] dataBoxes = null)
        {
            InitializeFromImpl(dataBoxes);
        }

        public static bool IsDepthStencilReadOnlySupported(GraphicsDevice device)
        {
            NullHelper.ToImplement();
            return false;
        }

        internal void SwapInternal(Texture other)
        {
            NullHelper.ToImplement();
        }

        private void InitializeFromImpl(DataBox[] dataBoxes = null)
        {
            NullHelper.ToImplement();
        }

        private void OnRecreateImpl()
        {
            NullHelper.ToImplement();
        }

        private bool IsFlipped()
        {
            NullHelper.ToImplement();
            return false;
        }

        internal static PixelFormat ComputeShaderResourceFormatFromDepthFormat(PixelFormat format)
        {
            NullHelper.ToImplement();
            return format;
        }
    }
} 
#endif
