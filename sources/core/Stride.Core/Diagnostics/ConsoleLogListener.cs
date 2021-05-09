// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

using Stride.Core.Annotations;

namespace Stride.Core.Diagnostics
{
    /// <summary>
    ///   Represents a <see cref="LogListener"/> implementation that redirects its output to the default console.
    ///   If this is not supported, the messages are output to <see cref="Debug"/>.
    /// </summary>
    public class ConsoleLogListener : LogListener
    {
        /// <summary>
        ///   Gets or sets the minimum log level handled by this listener.
        /// </summary>
        /// <value>The minimum log level.</value>
        public LogMessageType LogLevel { get; set; }

        /// <summary>
        ///   Gets or sets the log mode.
        /// </summary>
        /// <value>The log mode.</value>
        public ConsoleLogMode LogMode { get; set; }

        protected override void OnLog([NotNull] ILogMessage logMessage)
        {
            // Filter logs with lower level
            if (!Debugger.IsAttached && // Always log when debugger is attached
                (logMessage.Type < LogLevel || LogMode == ConsoleLogMode.None ||
                ((LogMode != ConsoleLogMode.Auto || !Platform.IsRunningDebugAssembly) && LogMode != ConsoleLogMode.Always)))
            {
                return;
            }

            // Make sure the console is opened when the debugger is not attached
            EnsureConsole();

            var exceptionMsg = GetExceptionText(logMessage);

            // Save initial console color
            var initialColor = Console.ForegroundColor;

            // Set the color depending on the message log level
            switch (logMessage.Type)
            {
                case LogMessageType.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;

                case LogMessageType.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case LogMessageType.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case LogMessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case LogMessageType.Error:
                case LogMessageType.Fatal:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            if (Debugger.IsAttached)
            {
                // Log the actual message
                Debug.WriteLine(GetDefaultText(logMessage));
                if (!string.IsNullOrEmpty(exceptionMsg))
                {
                    Debug.WriteLine(exceptionMsg);
                }
            }

            // Log the actual message
            Console.WriteLine(GetDefaultText(logMessage));
            if (!string.IsNullOrEmpty(exceptionMsg))
            {
                Console.WriteLine(exceptionMsg);
            }

            // Revert console initial color
            Console.ForegroundColor = initialColor;
        }

        // TODO: MOVE THIS CODE OUT IN A SEPARATE CLASS

        private bool isConsoleActive;
        private void EnsureConsole()
        {
            if (isConsoleActive)
                return;

            // Try to attach to the parent console, if the program is run directly from a console
            var attachedToConsole = AttachConsole(-1);
            if (!attachedToConsole)
            {
                // Else open a new console
                ShowConsole();
            }

            isConsoleActive = true;
        }

        public static void ShowConsole()
        {
            var handle = GetConsoleWindow();

            var outputRedirected = IsHandleRedirected((IntPtr) StdOutConsoleHandle);

            // If we are outputting somewhere unexpected, add an additional console window
            if (outputRedirected)
            {
                var originalStream = Console.OpenStandardOutput();

                // Free before trying to allocate
                FreeConsole();
                AllocConsole();

                var outputStream = Console.OpenStandardOutput();
                if (originalStream != null)
                {
                    outputStream = new DualStream(originalStream, outputStream);
                }

                TextWriter writer = new StreamWriter(outputStream) { AutoFlush = true };
                Console.SetOut(writer);
            }
            else if (handle != IntPtr.Zero)
            {
                const int SW_SHOW = 5;
                ShowWindow(handle, SW_SHOW);
            }
        }

        private class DualStream : Stream
        {
            private readonly Stream stream1;
            private readonly Stream stream2;

            public DualStream(Stream stream1, Stream stream2)
            {
                this.stream1 = stream1;
                this.stream2 = stream2;
            }

            public override void Flush()
            {
                stream1.Flush();
                stream2.Flush();
            }

            public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

            public override void SetLength(long value) => throw new NotImplementedException();

            public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();

            public override void Write(byte[] buffer, int offset, int count)
            {
                stream1.Write(buffer, offset, count);
                stream2.Write(buffer, offset, count);
            }

            public override bool CanRead => false;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length => throw new NotImplementedException();

            public override long Position { get; set; }
        }

        public static void HideConsole()
        {
            var handle = GetConsoleWindow();
            const int SW_HIDE = 0;
            ShowWindow(handle, SW_HIDE);
        }

        private const int StdOutConsoleHandle = -11;

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        private static extern int GetFileType(SafeFileHandle handle);

        private static bool IsHandleRedirected(IntPtr ioHandle)
        {
            if ((GetFileType(new SafeFileHandle(ioHandle, ownsHandle: false)) & 2) != 2)
                return true;

            // We are fine with being attached to non-consoles
            return false;

            //int mode;
            //return !GetConsoleMode(ioHandle, out mode);
        }
    }
}
