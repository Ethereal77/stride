// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

using Stride.Core.BuildEngine;
using Stride.Core.Diagnostics;
using Stride.Core.Serialization.Contents;
using Stride.Core.Storage;

namespace Stride.Core.Assets.CompilerApp
{
    [ServiceContract]
    public interface IProcessBuilderRemote
    {
        [OperationContract]
        [UseStrideDataContractSerializer]
        Command GetCommandToExecute();

        [OperationContract]
        [UseStrideDataContractSerializer]
        void ForwardLog(SerializableLogMessage message);

        [OperationContract]
        [UseStrideDataContractSerializer]
        void RegisterResult(CommandResultEntry commandResult);

        [OperationContract]
        [UseStrideDataContractSerializer]
        ObjectId ComputeInputHash(UrlType type, string filePath);

        [OperationContract]
        [UseStrideDataContractSerializer]
        Dictionary<ObjectUrl, ObjectId> GetOutputObjects();

        [OperationContract]
        [UseStrideDataContractSerializer]
        List<string> GetAssemblyContainerLoadedAssemblies();
    }
}
