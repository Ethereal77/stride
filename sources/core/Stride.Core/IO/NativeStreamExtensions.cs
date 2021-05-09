// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.IO;

using Stride.Core.Annotations;

namespace Stride.Core.IO
{
    /// <summary>
    /// Extension methods concerning <see cref="NativeStream"/>.
    /// </summary>
    public static class NativeStreamExtensions
    {
        /// <summary>
        /// Converts a <see cref="Stream"/> to a <see cref="NativeStream"/>.
        /// </summary>
        /// <remarks>
        /// If <see cref="stream"/> is already a <see cref="NativeStream"/>, it will be returned as is.
        /// Otherwise, a <see cref="NativeStreamWrapper"/> around it will be created.
        /// </remarks>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        [NotNull]
        public static NativeStream ToNativeStream(this Stream stream)
        {
            var nativeStream = stream as NativeStream;
            if (nativeStream == null)
                nativeStream = new NativeStreamWrapper(stream);

            return nativeStream;
        }
    }
}
