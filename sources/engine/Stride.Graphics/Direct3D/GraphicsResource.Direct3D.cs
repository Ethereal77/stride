// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_DIRECT3D11

using SharpDX.Direct3D11;

namespace Stride.Graphics
{
    public abstract partial class GraphicsResource
    {
        private ShaderResourceView shaderResourceView;
        private UnorderedAccessView unorderedAccessView;

        // Used to internally to force a WriteDiscard (to force a rename) with the GraphicsResourceAllocator
        internal bool DiscardNextMap;

        protected bool IsDebugMode => GraphicsDevice?.IsDebugMode ?? false;

        protected override void OnNameChanged()
        {
            base.OnNameChanged();

            if (IsDebugMode)
            {
                if (shaderResourceView != null)
                    shaderResourceView.DebugName = Name is null ? null : $"{Name} SRV";

                if (unorderedAccessView != null)
                    unorderedAccessView.DebugName = Name is null ? null : $"{Name} UAV";
            }
        }

        /// <summary>
        ///   Gets or sets the shader resource view attached to this graphics resource.
        /// </summary>
        /// <value>The shader resource view.</value>
        /// <remarks>
        ///   Note that only Texture, Texture3D, RenderTarget2D, RenderTarget3D, and DepthStencil are using this
        ///   ShaderResourceView.
        /// </remarks>
        protected internal ShaderResourceView NativeShaderResourceView
        {
            get => shaderResourceView;

            set
            {
                shaderResourceView = value;

                if (IsDebugMode && shaderResourceView != null)
                {
                    shaderResourceView.DebugName = Name == null ? null : $"{Name} SRV";
                }
            }
        }

        /// <summary>
        ///   Gets or sets the unordered access view attached to this graphics resource.
        /// </summary>
        /// <value>The unordered access view.</value>
        protected internal UnorderedAccessView NativeUnorderedAccessView
        {
            get => unorderedAccessView;

            set
            {
                unorderedAccessView = value;

                if (IsDebugMode && unorderedAccessView != null)
                {
                    unorderedAccessView.DebugName = Name == null ? null : $"{Name} UAV";
                }
            }
        }

        protected internal override void OnDestroyed()
        {
            ReleaseComObject(ref shaderResourceView);
            ReleaseComObject(ref unorderedAccessView);

            base.OnDestroyed();
        }
    }
}

#endif
