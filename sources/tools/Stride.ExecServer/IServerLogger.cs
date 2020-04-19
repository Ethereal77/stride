// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.ServiceModel;

namespace Xenko.ExecServer
{
    /// <summary>
    /// Interface used to log back standard output and error to client.
    /// </summary>
    [ServiceContract]
    public interface IServerLogger
    {
        [OperationContract(IsOneWay = true)]
        void OnLog(string text, ConsoleColor color);
    }
}
