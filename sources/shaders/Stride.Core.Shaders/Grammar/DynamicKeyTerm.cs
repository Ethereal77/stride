// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Irony.Parsing;

namespace Xenko.Core.Shaders.Grammar
{
    internal abstract class DynamicKeyTerm : KeyTerm
    {
        protected DynamicKeyTerm(string text, string name)
            : base(text, name)
        {
        }

        public abstract void Match(Tokenizer toknizer, out Token token);
    }
}
