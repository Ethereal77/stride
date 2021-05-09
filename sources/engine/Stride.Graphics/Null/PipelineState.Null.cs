// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_GRAPHICS_API_NULL 

namespace Stride.Graphics
{
    public partial class PipelineState
    {
        /// <summary>
        /// Initializes new instance of <see cref="PipelineState"/> for <param name="device"/>
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <param name="pipelineStateDescription">The pipeline state description.</param>
        private PipelineState(GraphicsDevice device, PipelineStateDescription pipelineStateDescription) : base(device)
        {
            NullHelper.ToImplement();
        }
    }
}

#endif
