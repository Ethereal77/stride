// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Rendering.Materials
{
    /// <summary>
    /// The texture coordinate.
    /// </summary>
    [DataContract("TextureCoordinate")]
    public enum TextureCoordinate
    {
        Texcoord0 = 0,
        Texcoord1 = 1,
        Texcoord2 = 2,
        Texcoord3 = 3,
        Texcoord4 = 4,
        Texcoord5 = 5,
        Texcoord6 = 6,
        Texcoord7 = 7,
        Texcoord8 = 8,
        Texcoord9 = 9,

        TexcoordNone,
    }
}
