// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core.Diagnostics;
using Stride.Core.Storage;
using Stride.Core.BuildEngine;
using Stride.Core.Serialization.Contents;

namespace Stride.Core.Assets.CompilerApp
{
    public interface IProcessBuilderRemote
    {
        Command GetCommandToExecute();

        void ForwardLog(SerializableLogMessage message);

        void RegisterResult(CommandResultEntry commandResult);

        ObjectId ComputeInputHash(UrlType type, string filePath);

        Dictionary<ObjectUrl, ObjectId> GetOutputObjects();

        List<string> GetAssemblyContainerLoadedAssemblies();
    }
}
