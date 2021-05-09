// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Linq;

using Mono.Cecil.Rocks;

using Stride.Core.AssemblyProcessor.Serializers;

namespace Stride.Core.AssemblyProcessor
{
    /// <summary>
    ///   Collects <c>DataContractAttribute.Alias</c> so that they are registered during Module initialization.
    /// </summary>
    internal class DataContractAliasProcessor : ICecilSerializerProcessor
    {
        public void ProcessSerializers(CecilSerializerContext context)
        {
            foreach (var type in context.Assembly.MainModule.GetAllTypes())
            {
                foreach (var dataContractAttribute in type.CustomAttributes.Where(x => x.AttributeType.FullName == "Stride.Core.DataContractAttribute" || x.AttributeType.FullName == "Stride.Core.DataAliasAttribute"))
                {
                    // Only process if ctor with 1 argument
                    if (!dataContractAttribute.HasConstructorArguments || dataContractAttribute.ConstructorArguments.Count != 1)
                        continue;

                    var alias = (string)dataContractAttribute.ConstructorArguments[0].Value;

                    // Third parameter is IsAlias (differentiate DataAlias from DataContract)
                    context.DataContractAliases.Add(Tuple.Create(alias, type, dataContractAttribute.AttributeType.FullName == "Stride.Core.DataAliasAttribute"));
                }
            }
        }
    }
}
