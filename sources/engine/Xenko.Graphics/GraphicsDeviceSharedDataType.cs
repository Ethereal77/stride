// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.Graphics
{
    /// <summary>
    /// Type of shared data. <see cref="GraphicsDevice.GetOrCreateSharedData{T}"/>
    /// </summary>
    public enum GraphicsDeviceSharedDataType
    {
        /// <summary>
        /// Data is shared within a <see cref="SharpDX.Direct3D11.Device"/>.
        /// </summary>
        PerDevice,

        /// <summary>
        /// Data is shared within a <see cref="SharpDX.Direct3D11.DeviceContext"/>
        /// </summary>
        PerContext,
    }
}
