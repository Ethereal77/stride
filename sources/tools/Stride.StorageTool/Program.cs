// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using System.Security.Principal;

using Microsoft.Win32;

using Mono.Options;

using static System.String;

namespace Stride.StorageTool
{
    /// <summary>
    ///   Tool to manage storage / bundles.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            var showHelp = false;
            int exitCode = 0;

            var p = new OptionSet
                {
                    "Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)",
                    "Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)",
                    "Storage Tool - Version: " + Format("{0}.{1}.{2}",
                        typeof(Program).Assembly.GetName().Version.Major,
                        typeof(Program).Assembly.GetName().Version.Minor,
                        typeof(Program).Assembly.GetName().Version.Build),
                    Empty,
                    Format("Usage: {0} command [options]*", exeName),
                    Empty,
                    "=== command ===",
                    Empty,
                    "view [bundleFile]",
                    "register",
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
                    Environment.Exit(0);
                }

                if (commandArgs.Count == 0)
                    throw new OptionException("Expecting a command", "");

                var command = commandArgs[0];
                switch (command)
                {
                    case "view":
                        if (commandArgs.Count != 2)
                            throw new OptionException("View command expecting a path to bundle file.", "");

                        StorageToolApp.View(commandArgs[1]);
                        break;

                    case "register":
                        // [HKEY_CURRENT_USER\Software\Classes\.bundle]
                        // @="bundlefile"
                        // [HKEY_CURRENT_USER\Software\Classes\bundlefile]
                        // @="Stride Bundle file Extension"
                        // [HKEY_CURRENT_USER\Software\Classes\bundlefile\shell\View\command]
                        // @="StorageTool.exe %1"

                        var classesKey = Registry.CurrentUser.OpenSubKey("Software\\Classes", RegistryKeyPermissionCheck.ReadWriteSubTree);
                        var bundleKey = classesKey.CreateSubKey(".bundle");
                        bundleKey.SetValue(null, "bundlefile");

                        var bundlefileKey = classesKey.CreateSubKey("bundlefile");
                        bundlefileKey.SetValue(null, "Stride Bundle file Extension");

                        var commandKey = bundlefileKey.CreateSubKey("shell").CreateSubKey("View").CreateSubKey("command");
                        commandKey.SetValue(null, Assembly.GetExecutingAssembly().Location + " view %1");
                        break;

                    default:
                        throw new OptionException(string.Format("Invalid command [{0}].", command), "");
                }
            }
            catch (Exception ex)
            {
                LogError("{0}: {1}", exeName, ex is OptionException || ex is StorageAppException ? e.Message : e.ToString());
                if (ex is OptionException)
                    p.WriteOptionDescriptions(Console.Out);
            }

            Environment.Exit(exitCode);
        }

        public static void LogError(string message, params object[] args)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message, args);
            Console.ForegroundColor = color;
        }
    }
}
