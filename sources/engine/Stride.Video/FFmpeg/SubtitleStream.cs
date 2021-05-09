// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

#if STRIDE_VIDEO_FFMPEG

using FFmpeg.AutoGen;

using Stride.Core.Annotations;

namespace Stride.Video.FFmpeg
{
    /// <summary>
    /// Represents a subtitle stream from a FFmpeg media.
    /// </summary>
    public sealed unsafe class SubtitleStream : FFmpegStream
    {
        public SubtitleStream([NotNull] AVStream* pStream, [NotNull] FFmpegMedia media)
            : base(pStream, media)
        {
        }

        /// <inheritdoc />
        public override FFMpegStreamType Type => FFMpegStreamType.Subtitle;
    }
}
#endif
