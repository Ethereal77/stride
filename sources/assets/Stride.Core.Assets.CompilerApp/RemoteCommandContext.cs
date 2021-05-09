// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core.BuildEngine;
using Stride.Core.Diagnostics;
using Stride.Core.Serialization.Contents;
using Stride.Core.Storage;

namespace Stride.Core.Assets.CompilerApp
{
    public class RemoteCommandContext : CommandContextBase
    {
        private readonly IProcessBuilderRemote processBuilderRemote;

        public RemoteCommandContext(IProcessBuilderRemote processBuilderRemote, Command command, BuilderContext builderContext, LoggerResult logger)
            : base(command, builderContext)
        {
            this.processBuilderRemote = processBuilderRemote;
            Logger = logger;
        }

        public override LoggerResult Logger { get; }

        internal new CommandResultEntry ResultEntry => base.ResultEntry;

        public override IEnumerable<IReadOnlyDictionary<ObjectUrl, OutputObject>> GetOutputObjectsGroups()
        {
            yield return processBuilderRemote.GetOutputObjects().ToDictionary(x => x.Key, x => new OutputObject(x.Key, x.Value));
        }

        public override ObjectId ComputeInputHash(UrlType type, string filePath)
        {
            return processBuilderRemote.ComputeInputHash(type, filePath);
        }
    }
}
