// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Shaders.Compiler
{
    [DataContract]
    public class RemoteEffectCompilerEffectRequested
    {
        // EffectCompileRequest serialized (so that it can be forwarded by EffectCompilerServer without being deserialized, since it might contain unknown types)
        public byte[] Request { get; set; }
    }
}
