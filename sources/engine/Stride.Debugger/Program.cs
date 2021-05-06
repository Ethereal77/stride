// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ServiceWire.NamedPipes;
using System.Threading;

using Mono.Options;

using Stride.Core;
using Stride.Debugger.Target;

namespace Stride
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            var showHelp = false;

            string hostPipe = null;
            bool waitDebuggerAttach = false;

            var p = new OptionSet
            {
                "Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)",
                "Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)",
                "Stride Debugger Host tool - Version: " + string.Format("{0}.{1}.{2}",
                    typeof(Program).Assembly.GetName().Version.Major,
                    typeof(Program).Assembly.GetName().Version.Minor,
                    typeof(Program).Assembly.GetName().Version.Build),
                string.Empty,
                $"Usage: {exeName} --host=[hostpipe]",
                string.Empty,
                "=== Options ===",
                string.Empty,
                { "h|help", "Show this message and exit", v => showHelp = v != null },
                { "host=", "Host pipe", v => hostPipe = v },
                { "wait-debugger-attach", "Process will wait for a debugger to attach, for 5 seconds", v => waitDebuggerAttach = true },
            };

            try
            {
                var unexpectedArgs = p.Parse(args);
                if (unexpectedArgs.Any())
                    throw new OptionException("Unexpected arguments [{0}]".ToFormat(string.Join(", ", unexpectedArgs)), "args");

                if (waitDebuggerAttach)
                {
                    // Wait for 2 second max
                    for (int i = 0; i < 500; ++i)
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            break;
                        }

                        Thread.Sleep(10);
                    }
                }

                if (hostPipe is null)
                    throw new OptionException("Host pipe not specified.", "host");

                // Open ServiceWire channel with master builder
                using var channel = new NpClient<IGameDebuggerHost>(new NpEndPoint(hostPipe));
                var gameDebuggerTarget = new GameDebuggerTarget();
                gameDebuggerTarget.MainLoop(channel.Proxy);
            }
            catch (OptionException ex)
            {
                Console.WriteLine("Command option '{0}': {1}", ex.OptionName, ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception: {0}", ex);
                return -1;
            }

            return 0;
        }
    }
}
