// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if XENKO_VIDEO_FFMPEG && !XENKO_GRAPHICS_API_DIRECT3D11

using Xenko.Core;
using Xenko.Core.Annotations;
using Xenko.Video.FFmpeg;

namespace Xenko.Video
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
