// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Threading;

using Xenko.Core.Storage;

namespace Xenko.Core.BuildEngine
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
