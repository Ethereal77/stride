// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Assets.Presentation.Preview
{
    /// <summary>
    /// Specify the different modes of preview of the TextureCube
    /// </summary>
    public enum TextureCubePreviewMode
    {
        /// <summary>
        /// Display only the Right texture
        /// </summary>
        Right = 0,

        /// <summary>
        /// Display only the Left texture
        /// </summary>
        Left = 1,

        /// <summary>
        /// Display only the Top texture
        /// </summary>
        Top = 2,

        /// <summary>
        /// Display only the Bottom texture
        /// </summary>
        Bottom = 3,

        /// <summary>
        /// Display only the front texture
        /// </summary>
        Front = 4,

        /// <summary>
        /// Display only the Back texture
        /// </summary>
        Back = 5,

        /// <summary>
        /// Display all the texture cube as template
        /// </summary>
        Full = 6,
    }
}
