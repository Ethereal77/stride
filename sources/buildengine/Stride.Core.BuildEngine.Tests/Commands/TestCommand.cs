// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.IO;

using Stride.Core.Serialization;

namespace Stride.Core.BuildEngine.Tests.Commands
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
