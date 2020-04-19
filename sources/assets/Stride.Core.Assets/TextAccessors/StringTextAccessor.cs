// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core;

namespace Xenko.Core.Assets.TextAccessors
{
    [DataContract]
    public class StringTextAccessor : ISerializableTextAccessor
    {
        [DataMember]
        public string Text { get; set; }

        public ITextAccessor Create()
        {
            var result = new DefaultTextAccessor();
            result.Set(Text);
            return result;
        }
    }
}
