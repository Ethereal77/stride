// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Storage;
using Xenko.Core.Diagnostics;
using Xenko.Core.Serialization.Contents;

namespace Xenko.Core.BuildEngine
{
    public interface ICommandContext
    {
        Command CurrentCommand { get; }

        LoggerResult Logger { get; }

        IEnumerable<IReadOnlyDictionary<ObjectUrl, OutputObject>> GetOutputObjectsGroups();

        void RegisterInputDependency(ObjectUrl url);

        void RegisterOutput(ObjectUrl url, ObjectId hash);

        void RegisterCommandLog(IEnumerable<ILogMessage> logMessages);

        void AddTag(ObjectUrl url, string tag);
    }
}
