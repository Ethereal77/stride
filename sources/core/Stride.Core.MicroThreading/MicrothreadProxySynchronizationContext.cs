// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Threading;

namespace Stride.Core.MicroThreading
{
    public class MicrothreadProxySynchronizationContext : SynchronizationContext, IMicroThreadSynchronizationContext
    {
        private readonly MicroThread microThread;

        public MicrothreadProxySynchronizationContext(MicroThread microThread)
        {
            this.microThread = microThread;
        }

        MicroThread IMicroThreadSynchronizationContext.MicroThread => microThread;
    }
}
