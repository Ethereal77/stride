// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

using Stride.Core;

namespace Stride.Shaders
{
    /// <summary>
    /// Structure containing shader bytecode, as well as mappings from input attribute locations to semantics.
    /// </summary>
    [DataContract]
    public struct ShaderInputBytecode
    {
        public Dictionary<int, string> InputAttributeNames;

        public Dictionary<string, int> ResourceBindings;

        public byte[] Data;
    }
}
