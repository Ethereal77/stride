// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core;

namespace Xenko.Shaders
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
