// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.ConnectionRouter
{
    /// <summary>
    /// Represents a connected device that the connection router is forwarding connections to.
    /// </summary>
    class ConnectedDevice
    {
        public object Key { get; set; }
        public string Name { get; set; }
        public bool DeviceDisconnected { get; set; }
    }
}
