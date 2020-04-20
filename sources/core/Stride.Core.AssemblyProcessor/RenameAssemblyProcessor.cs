// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Mono.Cecil;

namespace Stride.Core.AssemblyProcessor
{
    internal class RenameAssemblyProcessor : IAssemblyDefinitionProcessor
    {
        private string assemblyName;

        public RenameAssemblyProcessor(string assemblyName)
        {
            this.assemblyName = assemblyName;
        }

        public bool Process(AssemblyProcessorContext context)
        {
            context.Assembly.Name.Name = assemblyName;
            context.Assembly.MainModule.Name = assemblyName + ".dll";

            return true;
        }
    }
}
