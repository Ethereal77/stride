// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;

using Stride.Core.Storage;
using Stride.Core.Diagnostics;
using Stride.Core.Serialization.Contents;

namespace Stride.Core.BuildEngine
{
    public interface IPrepareContext
    {
        Logger Logger { get; }
        ObjectId ComputeInputHash(UrlType type, string filePath);
    }

    public interface IExecuteContext : IPrepareContext
    {
        CancellationTokenSource CancellationTokenSource { get; }
        ObjectDatabase ResultMap { get; }
        Dictionary<string, string> Variables { get; }

        void ScheduleBuildStep(BuildStep step);

        IEnumerable<IReadOnlyDictionary<ObjectUrl, OutputObject>> GetOutputObjectsGroups();

        CommandBuildStep IsCommandCurrentlyRunning(ObjectId commandHash);
        void NotifyCommandBuildStepStarted(CommandBuildStep commandBuildStep, ObjectId commandHash);
        void NotifyCommandBuildStepFinished(CommandBuildStep commandBuildStep, ObjectId commandHash);
    }
}
