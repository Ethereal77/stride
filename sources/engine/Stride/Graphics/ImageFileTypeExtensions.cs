// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma warning disable SA1402 // File may only contain a single type

namespace Stride.Graphics
{
    public static class ImageFileTypeExtensions
    {
        /// <summary>
        /// Return the file extension corresponding to the image file type.
        /// </summary>
        /// <param name="fileType">The file type</param>
        /// <returns>The file extension (for example ".png").</returns>
        public static string ToFileExtension(this ImageFileType fileType)
        {
            switch (fileType)
            {
                case ImageFileType.Stride:
                    return ".sdimg";
                default:
                    return "." + fileType.ToString().ToLowerInvariant();
            }
        }
    }
}
