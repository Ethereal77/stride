// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Graphics;

namespace Stride.Games
{
    public interface IGraphicsDeviceFactory
    {
        List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters graphicsParameters);

        GraphicsDevice ChangeOrCreateDevice(GraphicsDevice currentDevice, GraphicsDeviceInformation deviceInformation);
    }
}
