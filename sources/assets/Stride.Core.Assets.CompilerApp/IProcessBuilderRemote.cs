// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

using Xenko.Core.BuildEngine;
using Xenko.Core.Diagnostics;
using Xenko.Core.Serialization.Contents;
using Xenko.Core.Storage;

namespace Xenko.Core.Assets.CompilerApp
{
    [ServiceContract]
    public interface IProcessBuilderRemote
    {
        [OperationContract]
        [UseXenkoDataContractSerializer]
        Command GetCommandToExecute();

        [OperationContract]
        [UseXenkoDataContractSerializer]
        void ForwardLog(SerializableLogMessage message);

        [OperationContract]
        [UseXenkoDataContractSerializer]
        void RegisterResult(CommandResultEntry commandResult);

        [OperationContract]
        [UseXenkoDataContractSerializer]
        ObjectId ComputeInputHash(UrlType type, string filePath);

        [OperationContract]
        [UseXenkoDataContractSerializer]
        Dictionary<ObjectUrl, ObjectId> GetOutputObjects();

        [OperationContract]
        [UseXenkoDataContractSerializer]
        List<string> GetAssemblyContainerLoadedAssemblies();
    }
}
