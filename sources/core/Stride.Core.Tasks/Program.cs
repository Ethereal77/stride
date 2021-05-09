// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Microsoft.Build.Locator;

using Mono.Options;

using Stride.Core.Assets.CompilerApp.Tasks;
using Stride.Core.Diagnostics;

using static System.String;

namespace Stride.Core.Tasks
{
    static class Program
    {
        public static int Main(string[] args)
        {
            MSBuildLocator.RegisterDefaults();
            return RealMain(args);
        }

        public static int RealMain(string[] args)
        {
            var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            var showHelp = false;

            var p = new OptionSet
            {
                "Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)",
                "Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)",
                "Stride MSBuild Tasks - Version: " + Format("{0}.{1}.{2}",
                    typeof(Program).Assembly.GetName().Version.Major,
                    typeof(Program).Assembly.GetName().Version.Minor,
                    typeof(Program).Assembly.GetName().Version.Build),
                Empty,
                Format("Usage: {0} command [options]*", exeName),
                Empty,
                "=== Commands ===",
                Empty,
                " locate-devenv <MSBuildPath>: Returns the path to Visual Studio 'devenv' executable.",
                " pack-assets <csprojFile> <intermediatePackagePath>: Copies and adjusts Assets for nupkg packaging.",
                Empty,
                "=== Options ===",
                Empty,
                { "h|help", "Shows this message and exits.", v => showHelp = v != null },
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
                if (commandArgs.Count == 0)
                    throw new OptionException("You need to specify a command.", "");

                switch (commandArgs[0])
                {
                    case "locate-devenv":
                    {
                        if (commandArgs.Count != 2)
                            throw new OptionException("Need one extra argument.", "");

                        var devenvPath = LocateDevenv.FindDevenv(commandArgs[1]);
                        if (devenvPath is null)
                        {
                            Console.WriteLine("Could not locate devenv.");
                            return 1;
                        }
                        Console.WriteLine(devenvPath);
                        break;
                    }
                    case "pack-assets":
                    {
                        if (commandArgs.Count != 3)
                            throw new OptionException("Need two extra arguments.", "");

                        var csprojFile = commandArgs[1];
                        var intermediatePackagePath = commandArgs[2];

                        var logger = new LoggerResult();
                        var generatedItems = new List<(string SourcePath, string PackagePath)>();

                        if (!PackAssetsHelper.Run(logger, csprojFile, intermediatePackagePath, generatedItems))
                        {
                            foreach (var message in logger.Messages)
                                Console.WriteLine(message);

                            return 1;
                        }
                        foreach (var generatedItem in generatedItems)
                            Console.WriteLine($"{generatedItem.SourcePath}|{generatedItem.PackagePath}");

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}", exeName, ex);
                if (ex is OptionException)
                    p.WriteOptionDescriptions(Console.Out);
                return 1;
            }

            return 0;
        }
    }
}
