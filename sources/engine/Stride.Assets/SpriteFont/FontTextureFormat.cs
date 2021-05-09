// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Assets.SpriteFont
{
    [DataContract]
    public enum FontTextureFormat
    {
        //Auto, -> currently not supported on all platforms 
        Rgba32,
        //CompressedMono, -> currently not supported on all platforms 
    }
}
