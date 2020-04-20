// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

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
