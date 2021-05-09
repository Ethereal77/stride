// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;

using Stride.Core.Storage;

namespace Stride.Core.BuildEngine
{
    public class BuilderContext
    {
        internal readonly Dictionary<ObjectId, CommandBuildStep> CommandsInProgress = new Dictionary<ObjectId, CommandBuildStep>();

        internal FileVersionTracker InputHashes { get; private set; }

        public CommandBuildStep.TryExecuteRemoteDelegate TryExecuteRemote { get; }

        public BuilderContext(FileVersionTracker inputHashes, CommandBuildStep.TryExecuteRemoteDelegate tryExecuteRemote)
        {
            InputHashes = inputHashes;
            TryExecuteRemote = tryExecuteRemote;
        }
    }
}
