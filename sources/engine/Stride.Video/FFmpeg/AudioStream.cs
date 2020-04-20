// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if STRIDE_VIDEO_FFMPEG

using FFmpeg.AutoGen;

using Stride.Core.Annotations;

namespace Stride.Video.FFmpeg
{
    /// <summary>
    /// Represents an audio stream from a FFmpeg media.
    /// </summary>
    public sealed unsafe class AudioStream : FFmpegStream
    {
        public AudioStream([NotNull] AVStream* pStream, [NotNull] FFmpegMedia media)
            : base(pStream, media)
        {
            var pCodec = pStream->codec;
            ChannelCount = pCodec->channels;
            SampleRate = pCodec->sample_rate;
        }

        /// <summary>
        /// The number of audio channels in the stream.
        /// </summary>
        public int ChannelCount { get; }

        /// <summary>
        /// The number of audio samples per second.
        /// </summary>
        public int SampleRate { get; }

        /// <inheritdoc />
        public override FFMpegStreamType Type => FFMpegStreamType.Audio;
    }
}
#endif
