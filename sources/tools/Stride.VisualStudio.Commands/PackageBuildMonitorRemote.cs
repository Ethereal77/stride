// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

using ServiceWire.NamedPipes;

using Stride.Core.BuildEngine;
using Stride.Core.Diagnostics;
using Stride.VisualStudio.Commands;

namespace Stride.VisualStudio.BuildEngine
{
    public class PackageBuildMonitorRemote : IForwardSerializableLogRemote
    {
        private readonly string logPipeUrl;
        private readonly IBuildMonitorCallback buildMonitorCallback;

        public PackageBuildMonitorRemote(IBuildMonitorCallback buildMonitorCallback, string logPipeUrl)
        {
            this.buildMonitorCallback = buildMonitorCallback;
            this.logPipeUrl = logPipeUrl;

            // Listen to pipe with this as listener
            var host = new NpHost(this.logPipeUrl, null, null, new StrideServiceWireSerializer());
            host.AddService<IForwardSerializableLogRemote>(this);
            host.Open();
        }

        public void ForwardSerializableLog(SerializableLogMessage message)
        {
            buildMonitorCallback.Message(message.Type.ToString(), message.Module, message.Text);
        }
    }
}
