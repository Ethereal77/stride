// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Graphics
{
    public class GraphicsException : Exception
    {
        public GraphicsException()
        {
        }

        public GraphicsException(string message, GraphicsDeviceStatus status = GraphicsDeviceStatus.Normal)
            : base(message)
        {
            Status = status;
        }

        public GraphicsException(string message, Exception innerException, GraphicsDeviceStatus status = GraphicsDeviceStatus.Normal)
            : base(message, innerException)
        {
            Status = status;
        }

        public GraphicsDeviceStatus Status { get; private set; }
    }
}
