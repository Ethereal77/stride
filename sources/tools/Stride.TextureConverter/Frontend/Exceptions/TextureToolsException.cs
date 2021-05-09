// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stride.TextureConverter
{
    public class TextureToolsException : ApplicationException
    {
        public TextureToolsException() : base() {}
        public TextureToolsException(string message) : base(message) {}
        public TextureToolsException(string message, System.Exception inner) : base(message, inner) {}
        protected TextureToolsException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
