// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Xenko.ConnectionRouter
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
