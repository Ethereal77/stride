// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.IO;

using Mono.Cecil;

using Stride.Core.Storage;

namespace Stride.Core.AssemblyProcessor
{
    internal class AssemblyProcessorContext
    {
        public CustomAssemblyResolver AssemblyResolver { get; private set; }
        public AssemblyDefinition Assembly { get; set; }
        public TextWriter Log { get; private set; }

        public ObjectId? SerializationHash { get; set; }

        public AssemblyProcessorContext(CustomAssemblyResolver assemblyResolver, AssemblyDefinition assembly, TextWriter log)
        {
            AssemblyResolver = assemblyResolver;
            Assembly = assembly;
            Log = log;
        }
    }
}
