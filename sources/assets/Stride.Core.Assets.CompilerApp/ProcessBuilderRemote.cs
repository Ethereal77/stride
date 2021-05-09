// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Stride.Core.BuildEngine;
using Stride.Core.Diagnostics;
using Stride.Core.Reflection;
using Stride.Core.Serialization.Contents;
using Stride.Core.Storage;

namespace Stride.Core.Assets.CompilerApp
{
    public class ProcessBuilderRemote : IProcessBuilderRemote
    {
        private readonly AssemblyContainer assemblyContainer;
        private readonly LocalCommandContext commandContext;
        private readonly Command remoteCommand;

        public CommandResultEntry Result { get; protected set; }

        public ProcessBuilderRemote(AssemblyContainer assemblyContainer, LocalCommandContext commandContext, Command remoteCommand)
        {
            this.assemblyContainer = assemblyContainer;
            this.commandContext = commandContext;
            this.remoteCommand = remoteCommand;
        }

        public Command GetCommandToExecute()
        {
            return remoteCommand;
        }

        public void RegisterResult(CommandResultEntry commandResult)
        {
            Result = commandResult;
        }

        public void ForwardLog(SerializableLogMessage message)
        {
            commandContext.Logger.Log(new LogMessage(message.Module, message.Type, message.Text));
            if (message.ExceptionInfo != null)
                commandContext.Logger.Log(new LogMessage(message.Module, message.Type, message.ExceptionInfo.ToString()));
        }

        public ObjectId ComputeInputHash(UrlType type, string filePath)
        {
            return commandContext.ComputeInputHash(type, filePath);
        }

        public Dictionary<ObjectUrl, ObjectId> GetOutputObjects()
        {
            var result = new Dictionary<ObjectUrl, ObjectId>();
            foreach (var outputObjects in commandContext.GetOutputObjectsGroups())
            {
                foreach (var outputObject in outputObjects)
                {
                    if (!result.ContainsKey(outputObject.Key))
                    {
                        result.Add(outputObject.Key, outputObject.Value.ObjectId);
                    }
                }
            }
            return result;
        }

        public List<string> GetAssemblyContainerLoadedAssemblies()
        {
            return assemblyContainer.LoadedAssemblies.Select(x => x.Path).ToList();
        }
    }
}
