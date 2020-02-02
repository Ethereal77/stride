// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.IO;

using Xenko.Core.Serialization;

namespace Xenko.Core.BuildEngine.Tests.Commands
{
    public abstract class TestCommand : Command
    {
        /// <inheritdoc/>
        public override string Title { get { return ToString(); } }

        private static int commandCounter;
        private readonly int commandId;

        public static void ResetCounter()
        {
            commandCounter = 0;
        }

        protected TestCommand()
        {
            commandId = ++commandCounter;
        }

        public override string ToString()
        {
            return GetType().Name + " " + commandId;
        }

        protected override void ComputeParameterHash(BinarySerializationWriter writer)
        {
            base.ComputeParameterHash(writer);

            writer.Write(commandId);
        }
    }
}
