// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace Xenko.Core.Serialization.Contents
{
    /// <summary>
    /// A subtype of <see cref="Exception"/> thrown by the <see cref="ContentManager"/>.
    /// </summary>
    internal class ContentManagerException : Exception
    {
        public ContentManagerException(string message) : base(message)
        {
        }

        public ContentManagerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
