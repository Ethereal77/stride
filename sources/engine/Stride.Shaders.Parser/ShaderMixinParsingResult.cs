// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;

using Xenko.Core.Storage;
using Xenko.Graphics;
using Xenko.Core.Shaders.Parser;

namespace Xenko.Shaders.Parser
{
    public class ShaderMixinParsingResult : ParsingResult
    {
        public ShaderMixinParsingResult()
        {
            EntryPoints = new Dictionary<ShaderStage, string>();
            HashSources = new HashSourceCollection();
        }

        public EffectReflection Reflection { get; set; }

        public Dictionary<ShaderStage, string> EntryPoints;

        public HashSourceCollection HashSources { get; set; }
    }
}
