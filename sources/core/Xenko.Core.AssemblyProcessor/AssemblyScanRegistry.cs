// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Mono.Cecil;

namespace Xenko.Core.AssemblyProcessor
{
    public class AssemblyScanRegistry
    {
        public void Register(TypeDefinition type, TypeReference scanType)
        {
            HashSet<TypeDefinition> types;
            if (!ScanTypes.TryGetValue(scanType, out types))
            {
                types = new HashSet<TypeDefinition>();
                ScanTypes.Add(scanType, types);
            }

            types.Add(type);
        }

        public bool HasScanTypes => ScanTypes.Count > 0;

        public Dictionary<TypeReference, HashSet<TypeDefinition>> ScanTypes { get; } = new Dictionary<TypeReference, HashSet<TypeDefinition>>(TypeReferenceEqualityComparer.Default);
    }
}
