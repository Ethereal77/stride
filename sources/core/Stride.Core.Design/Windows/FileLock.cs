// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Threading;

using Stride.Core.Annotations;
using Stride.Core.IO;

namespace Stride.Core.Windows
{
    /// <summary>
    ///   Represents a thread-safe, process-safe file lock.
    /// </summary>
    public sealed class FileLock : IDisposable
    {
        private FileStream lockFile;

        /// <summary>
        ///   Initializes a new instance of the <see cref="FileLock"/> class.
        /// </summary>
        /// <param name="lockFile">The file to serve as a lock.</param>
        /// <exception cref="ArgumentNullException"><paramref name="lockFile"/> is a <c>null</c> reference.</exception>
        private FileLock(FileStream lockFile)
        {
            this.lockFile = lockFile ?? throw new ArgumentNullException(nameof(lockFile));
        }

        /// <summary>
        ///   Releases the file lock.
        /// </summary>
        public void Dispose()
        {
            if (lockFile is not null)
            {
                var overlapped = new NativeOverlapped();
                NativeLockFile.UnlockFileEx(lockFile.SafeFileHandle, 0, uint.MaxValue, uint.MaxValue, ref overlapped);
                lockFile.Dispose();

                // Try to delete the file
                // Ideally we would use FileOptions.DeleteOnClose, but it doesn't seem to work well with FileShare for second instance
                try
                {
                    File.Delete(lockFile.Name);
                }
                catch { }

                lockFile = null;
            }
        }

        /// <summary>
        ///   Tries to take ownership of a file lock without waiting.
        /// </summary>
        /// <param name="name">A unique name identifying the file lock.</param>
        /// <returns>
        ///   A new instance of <see cref="FileLock"/> if the ownership could be taken; <c>null</c> otherwise.
        /// </returns>
        /// <remarks>
        ///   The returned <see cref="FileLock"/> must be disposed to release the lock.
        /// </remarks>
        [CanBeNull]
        public static FileLock TryLock(string name) => WaitInternal(name, timeout: 0);

        /// <summary>
        ///   Waits indefinitely to take ownership of a file lock.
        /// </summary>
        /// <param name="name">A unique name identifying the file lock.</param>
        /// <returns>
        ///   A new instance of <see cref="FileLock"/> if the ownership could be taken; <c>null</c> otherwise.
        /// </returns>
        /// <remarks>
        ///   The returned <see cref="FileLock"/> must be disposed to release the lock.
        /// </remarks>
        [CanBeNull]
        public static FileLock Wait(string name) => WaitInternal(name, timeout: -1);

        //
        // Tries to take ownership of a file lock.
        //
        private static FileLock WaitInternal(string name, int timeout)
        {
            var fileLock = BuildFileLock(name);
            try
            {
                if (timeout != 0 && timeout != -1)
                    throw new NotImplementedException("FileLock.Wait() is implemented only for timeouts 0 or -1.");

                var overlapped = new NativeOverlapped();
                bool hasHandle = NativeLockFile.LockFileEx(
                    fileLock.SafeFileHandle,
                    NativeLockFile.LOCKFILE_EXCLUSIVE_LOCK | (timeout == 0 ? NativeLockFile.LOCKFILE_FAIL_IMMEDIATELY : 0),
                    reserved: 0,
                    countLow: uint.MaxValue,
                    countHigh: uint.MaxValue,
                    ref overlapped);

                return !hasHandle ? null : new FileLock(fileLock);
            }
            catch (AbandonedMutexException)
            {
                return new FileLock(fileLock);
            }

            //
            // Creates a file to be used as lock.
            //
            static FileStream BuildFileLock(string name)
            {
                // We open with FileShare.ReadWrite mode so that we can implement `Wait`.
                return new FileStream(name, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            }
        }
    }
}
