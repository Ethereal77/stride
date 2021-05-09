// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_VIDEO_FFMPEG && !STRIDE_GRAPHICS_API_DIRECT3D11

using Stride.Core;
using Stride.Core.Annotations;
using Stride.Video.FFmpeg;

namespace Stride.Video
{
    public partial class VideoSystem
    {
        public override void Initialize()
        {
            base.Initialize();

            // Initialize ffmpeg
            FFmpegUtils.PreloadLibraries();
            FFmpegUtils.Initialize();
        }
    }
}

#endif
