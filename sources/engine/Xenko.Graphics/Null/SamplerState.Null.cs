// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_GRAPHICS_API_NULL 

using System;

namespace Xenko.Graphics
{
    public partial class SamplerState
    {
        private SamplerState(GraphicsDevice graphicsDevice, SamplerStateDescription samplerStateDescription)
        {
            throw new NotImplementedException();
        }
    }
} 
#endif 
