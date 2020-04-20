// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.IO;

namespace Stride.Core.Translation.Extractor
{
    internal class Message
    {
        public string Comment;
        public string Context;
        public string PluralText;
        public string Text;

        public long LineNumber;
        public UFile Source;
    }
}
