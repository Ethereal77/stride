// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.ServiceModel;

using Xenko.Core.Diagnostics;

namespace Xenko.Debugger.Target
{
    /// <summary>
    /// Represents the debugger host commands that the target can access
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IGameDebuggerTarget))]
    public interface IGameDebuggerHost
    {
        [OperationContract]
        void RegisterTarget();

        [OperationContract]
        void OnGameExited();

        [OperationContract(IsOneWay = true)]
        void OnLogMessage(SerializableLogMessage logMessage);
    }
}
