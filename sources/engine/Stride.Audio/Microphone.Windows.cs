// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Audio
{
    internal partial class Microphone
    {
        /// <summary>
        /// Create a new instance of Microphone ready for recording.
        /// </summary>
        /// <exception cref="NoMicrophoneConnectedException">No microphone is currently plugged.</exception>
        public Microphone()
        {
            throw new NotImplementedException();
        }

        #region Implementation of the IRecorder interface

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
