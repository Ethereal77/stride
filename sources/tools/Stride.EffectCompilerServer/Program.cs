// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using System.Threading;

using Mono.Options;

using Stride.Engine.Network;

using static System.String;

namespace Stride.EffectCompilerServer
{
    class Program
    {
        static int Main(string[] args)
        {
            var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            var showHelp = false;
            int exitCode = 0;

            var p = new OptionSet
                {
                    "Copyright (c) Stride and its contributors (https://stride3d.net)",
                    "Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)",
                    "Stride Effect Compiler Server - Version: " + Format("{0}.{1}.{2}",
                        typeof(Program).Assembly.GetName().Version.Major,
                        typeof(Program).Assembly.GetName().Version.Minor,
                        typeof(Program).Assembly.GetName().Version.Build),
                    Empty,
                    Format("Usage: {0}", exeName),
                    Empty,
                    "=== Options ===",
                    Empty,
                    { "h|help", "Show this message and exit", v => showHelp = v != null },
                };

            try
            {
                var commandArgs = p.Parse(args);
                if (showHelp)
                {
                    p.WriteOptionDescriptions(Console.Out);
                    return 0;
                }

                // Make sure path exists
                if (commandArgs.Count > 0)
                    throw new OptionException("This command expect no additional arguments", "");

                var effectCompilerServer = new EffectCompilerServer();
                effectCompilerServer.TryConnect("127.0.0.1", RouterClient.DefaultPort);

                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                {
                    var ex = eventArgs.ExceptionObject as Exception;
                    if (ex is null)
                        return;

                    Console.WriteLine($"Unhandled Exception: {ex.Message.ToString()}");
                };

                // Forbid process to terminate (unless ctrl+c)
                while (true)
                {
                    Console.Read();
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}", exeName, ex);
                if (ex is OptionException)
                    p.WriteOptionDescriptions(Console.Out);
                exitCode = 1;
            }

            return exitCode;
        }
    }
}
