// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#if STRIDE_VIDEO_FFMPEG

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using FFmpeg.AutoGen;

using Stride.Core;
using Stride.Core.Annotations;

namespace Stride.Video.FFmpeg
{
    /// <summary>
    /// Collection of utilities when invoking <see cref="global::FFmpeg.AutoGen"/>.
    /// </summary>
    public static class FFmpegUtils
    {
        private static volatile bool initialized = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckPlatformSupport() => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnsurePlatformSupport()
        {
            if (!CheckPlatformSupport())
                throw new PlatformNotSupportedException();
        }

        /// <summary>
        ///   Initializes FFmpeg.
        /// </summary>
        public static void Initialize()
        {
            if (!CheckPlatformSupport() || initialized)
                return;

            initialized = true;
            ffmpeg.av_register_all();
            ffmpeg.avcodec_register_all();
            ffmpeg.avformat_network_init();
        }

        /// <summary>
        ///   Preloads all FFmpeg libraries.
        /// </summary>
        /// <remarks>
        ///   Must be called before any attempt to use FFmpeg API or this will have no effect.
        /// </remarks>
        public static void PreloadLibraries()
        {
            if (!CheckPlatformSupport() || initialized)
                return;

            // Note: order matters (dependencies)
            // avdevice
            //   |---- avfilter
            //   |       |---- avformat
            //   |       |       |---- avcodec
            //   |       |       |       |---- swresample
            //   |       |       |       |       |---- avutil
            //   |       |       |       |---- avutil
            //   |       |       |---- avutil
            //   |       |---- swscale
            //   |       |       |---- avutil
            //   |       |---- swresample
            //   |       |---- avutil
            //   |---- avformat
            //   |---- avcodec
            //   |---- avutil
            var type = typeof(FFmpegUtils);
            NativeLibraryHelper.PreloadLibrary("avutil-55", type);
            NativeLibraryHelper.PreloadLibrary("swresample-2", type);
            NativeLibraryHelper.PreloadLibrary("avcodec-57", type);
            NativeLibraryHelper.PreloadLibrary("avformat-57", type);
            NativeLibraryHelper.PreloadLibrary("swscale-4", type);
            NativeLibraryHelper.PreloadLibrary("avfilter-6", type);
            NativeLibraryHelper.PreloadLibrary("avdevice-57", type);
        }

        /// <summary>
        ///   Converts a <see cref="AVDictionary"/>* to a <see cref="Dictionary{string, string}"/>.
        /// </summary>
        /// <param name="avDictionary">A pointer to a <see cref="AVDictionary"/> struct.</param>
        /// <returns>A new dictionary containing a copy of all entries.</returns>
        [NotNull]
        internal static unsafe Dictionary<string, string> ToDictionary(AVDictionary* avDictionary)
        {
            var dictionary = new Dictionary<string, string>();
            if (avDictionary == null)
                return dictionary;

            AVDictionaryEntry* tag = null;
            while ((tag = ffmpeg.av_dict_get(avDictionary, string.Empty, tag, ffmpeg.AV_DICT_IGNORE_SUFFIX)) != null)
            {
                var key = Marshal.PtrToStringAnsi((IntPtr)tag->key);
                var value = Marshal.PtrToStringAnsi((IntPtr)tag->value);
                dictionary.Add(key, value);
            }
            return dictionary;
        }
    }
}

#endif
