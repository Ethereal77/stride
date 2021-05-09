// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Assets;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Assets;
using Stride.Graphics;

namespace Stride.Editor.Thumbnails
{
    /// <summary>
    /// The minimum parameters needed by a thumbnail build command.
    /// </summary>
    [DataContract]
    public class ThumbnailCommandParameters
    {
        public ThumbnailCommandParameters()
        {
        }

        public ThumbnailCommandParameters(Asset asset, string thumbnailUrl, Int2 thumbnailSize)
        {
            Asset = asset;
            ThumbnailUrl = thumbnailUrl;
            ThumbnailSize = thumbnailSize;
        }

        public Asset Asset;
        
        public string ThumbnailUrl; // needed to force re-calculation of thumbnails when asset file is move

        public Int2 ThumbnailSize;

        public ColorSpace ColorSpace { get; set; }

        public RenderingMode RenderingMode { get; set; }
    }
}
