// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Audio
{
    /// <summary>
    /// Class implementing the IRecoder interface designed to capture microphone audio input. Refer to <see cref="IRecorder"/> for more details.
    /// </summary>
    internal sealed partial class Microphone : IRecorder
    {
        #region Implementation of the IRecorder interface

        public TimeSpan BufferDuration { get; set; }

        public TimeSpan BufferSize { get; set; }

        public int SampleRate { get; private set; }

        public RecorderState State { get; private set; }

        public TimeSpan GetSampleDuration(int sizeInBytes)
        {
            throw new NotImplementedException();
        }

        public int GetSampleSizeInBytes(TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        public int GetData(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public int GetData(byte[] buffer, int offset, int count)
        {
            // Just to avoid warning on BufferReady, code to remove
            BufferReady?.Invoke(this, EventArgs.Empty);
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> BufferReady;

        #endregion
    }
}
