// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;

namespace Stride.Core.IO
{
    public partial class TemporaryFile : IDisposable
    {
        private bool isDisposed;

        public string Path { get; private set; }


        public TemporaryFile()
        {
            Path = VirtualFileSystem.GetTempFileName();
        }

        ~TemporaryFile()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                TryDelete();
            }
        }

        private void TryDelete()
        {
            try
            {
                VirtualFileSystem.FileDelete(Path);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
}
