// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Stride.Core
{
    public class ServiceEventArgs : EventArgs
    {
        public ServiceEventArgs(Type serviceType, object serviceInstance)
        {
            ServiceType = serviceType;
            Instance = serviceInstance;
        }

        public Type ServiceType { get; private set; }

        public object Instance { get; private set; }
    }
}
