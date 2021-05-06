// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using Mono.Options;

using Stride.Core.IO;
using Stride.Core.Yaml;
using Stride.Core.Diagnostics;
using Stride.Core.BuildEngine;
using Stride.Core.Assets.Diagnostics;
using Stride.Core.Assets.CompilerApp.Tasks;
using Stride.Core.VisualStudio;
using Stride.Assets.Models;
using Stride.Assets.SpriteFont;
using Stride.Rendering.Materials;
using Stride.Rendering.ProceduralModels;
using Stride.Particles;
using Stride.SpriteStudio.Offline;

namespace Stride.Core.Assets.CompilerApp
{
    class PackageBuilderApp : IPackageBuilderApp
    {
        private static Stopwatch clock;

        private LogListener globalLoggerOnGlobalMessageLogged;

        private PackageBuilder builder;

        public bool IsSlave { get; private set; }

        public int Run(string[] args)
        {
            clock = Stopwatch.StartNew();

            // TODO: This is hardcoded. Check how to make this dynamic instead.
            RuntimeHelpers.RunModuleConstructor(typeof(IProceduralModel).Module.ModuleHandle);
            RuntimeHelpers.RunModuleConstructor(typeof(MaterialKeys).Module.ModuleHandle);
            RuntimeHelpers.RunModuleConstructor(typeof(SpriteFontAsset).Module.ModuleHandle);
            RuntimeHelpers.RunModuleConstructor(typeof(ModelAsset).Module.ModuleHandle);
            RuntimeHelpers.RunModuleConstructor(typeof(SpriteStudioAnimationAsset).Module.ModuleHandle);
            RuntimeHelpers.RunModuleConstructor(typeof(ParticleSystem).Module.ModuleHandle);
            //var project = new Package();
            //project.Save("test.sdpkg");

            //Thread.Sleep(10000);
            //var spriteFontAsset = StaticFontAsset.New();
            //Content.Save("test.sdfnt", spriteFontAsset);
            //project.Refresh();

            //args = new string[] { "test.sdpkg", "-o:app_data", "-b:tmp", "-t:1" };

            var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            var showHelp = false;
            var packMode = false;
            var updateGeneratedFilesMode = false;
            var buildEngineLogger = GlobalLogger.GetLogger("BuildEngine");
            var options = new PackageBuilderOptions(new ForwardingLoggerResult(buildEngineLogger));

            var p = new OptionSet
            {
                "Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)",
                "Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)",
                "Stride Build Tool - Version: "
                +
                String.Format(
                    "{0}.{1}.{2}",
                    typeof(Program).Assembly.GetName().Version.Major,
                    typeof(Program).Assembly.GetName().Version.Minor,
                    typeof(Program).Assembly.GetName().Version.Build) + string.Empty,
                string.Format("Usage: {0} inputPackageFile [options]* -b buildPath", exeName),
                string.Empty,
                "=== Options ===",
                string.Empty,
                { "h|help", "Show this message and exit", help => showHelp = help != null },
                { "v|verbose", "Show more verbose progress logs", verbose => options.Verbose = verbose != null },
                { "d|debug", "Show debug logs (imply verbose)", debug => options.Debug = debug != null },
                { "log", "Enable file logging", enableLogging => options.EnableFileLogging = enableLogging != null },
                { "disable-auto-compile", "Disable auto-compile of projects", disableAutoCompile => options.DisableAutoCompileProjects = disableAutoCompile != null},
                { "project-configuration=", "Project configuration", config => options.ProjectConfiguration = config },
                { "platform=", "Platform name", platform => options.Platform = (PlatformType) Enum.Parse(typeof(PlatformType), platform) },
                { "solution-file=", "Solution File Name", solutionFile => options.SolutionFile = solutionFile },
                { "package-id=", "Package Id from the solution file", packageId => options.PackageId = Guid.Parse(packageId) },
                { "package-file=", "Input Package File Name", packageFile => options.PackageFile = packageFile },
                { "msbuild-uptodatecheck-filebase=", "BuildUpToDate File base for MSBuild; it will create a .inputs and a .outputs file", fileBase => options.MSBuildUpToDateCheckFileBase = fileBase },
                { "o|output-path=", "Output path", outputDir => options.OutputDirectory = outputDir },
                { "b|build-path=", "Build path", buildDir => options.BuildDirectory = buildDir },
                { "log-file=", "Log build in a custom file.", logFile =>
                {
                    options.EnableFileLogging = logFile != null;
                    options.CustomLogFileName = logFile;
                } },
                { "monitor-pipe=", "Monitor pipe.", pipe =>
                {
                    if (!string.IsNullOrEmpty(pipe))
                        options.MonitorPipeNames.Add(pipe);
                } },
                { "slave=", "Slave pipe", pipe => options.SlavePipe = pipe }, // Benlitz: I don't think this should be documented
                { "server=", "This Compiler is launched as a server", _ => { } },
                { "pack", "Special mode to copy assets and resources in a folder for NuGet packaging", _ => packMode = true },
                { "updated-generated-files", "Special mode to update generated files (such as .sdsl.cs)", u => updateGeneratedFilesMode = true },
                { "t|threads=", "Number of threads to create. Default value is the number of hardware threads available.", t => options.ThreadCount = int.Parse(t) },
                { "test=", "Run a test session.", v => options.TestName = v },
                { "property:", "Properties. Format is name1=value1;name2=value2", properties =>
                {
                    if (!string.IsNullOrEmpty(properties))
                    {
                        foreach (var nameValue in properties.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            var equalIndex = nameValue.IndexOf('=');
                            if (equalIndex == -1)
                                throw new OptionException("Expected name1=value1;name2=value2 format.", "property");

                            var name = nameValue.Substring(0, equalIndex);
                            var value = nameValue[(equalIndex + 1)..];
                            if (value != string.Empty)
                                options.Properties.Add(name, value);
                        }
                    }
                }
                },
                { "compile-property:", "Compile properties. Format is name1=value1;name2=value2", v =>
                {
                    if (!string.IsNullOrEmpty(v))
                    {
                        if (options.ExtraCompileProperties is null)
                            options.ExtraCompileProperties = new Dictionary<string, string>();

                        foreach (var nameValue in v.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            var equalIndex = nameValue.IndexOf('=');
                            if (equalIndex == -1)
                                throw new OptionException("Expected name1=value1;name2=value2 format.", "property");

                            options.ExtraCompileProperties.Add(nameValue.Substring(0, equalIndex), nameValue[(equalIndex + 1)..]);
                        }
                    }
                }
                },
                {
                    "reattach-debugger=", "Reattach to a Visual Studio debugger", v =>
                    {
                        if (!string.IsNullOrEmpty(v) && int.TryParse(v, out var debuggerProcessId))
                        {
                            if (!Debugger.IsAttached)
                            {
                                using var debugger = VisualStudioDebugger.GetByProcess(debuggerProcessId);
                                debugger?.Attach();
                            }
                        }
                    }
                },
            };

            TextWriterLogListener fileLogListener = null;

            BuildResultCode exitCode;

            try
            {
                var unexpectedArgs = p.Parse(args);

                // Activate proper log level
                buildEngineLogger.ActivateLog(options.LoggerType);

                // Output logs to the console with colored messages
                if (options.SlavePipe is null)
                {
                    globalLoggerOnGlobalMessageLogged = new ConsoleLogListener { LogMode = ConsoleLogMode.Always };
                    globalLoggerOnGlobalMessageLogged.TextFormatter = FormatLog;
                    GlobalLogger.GlobalMessageLogged += globalLoggerOnGlobalMessageLogged;
                }

                if (updateGeneratedFilesMode)
                {
                    PackageSessionPublicHelper.FindAndSetMSBuildVersion();

                    var csprojFile = options.PackageFile;

                    var logger = new LoggerResult();
                    var projectDirectory = new UDirectory(Path.GetDirectoryName(csprojFile));
                    var package = Package.Load(logger, csprojFile, new PackageLoadParameters()
                    {
                        LoadMissingDependencies = false,
                        AutoCompileProjects = false,
                        LoadAssemblyReferences = false,
                        AutoLoadTemporaryAssets = true,
                        TemporaryAssetFilter = assetFile => AssetRegistry.IsProjectCodeGeneratorAssetFileExtension(assetFile.FilePath.GetFileExtension().ToLowerInvariant())
                    });

                    foreach (var assetItem in package.TemporaryAssets)
                    {
                        if (assetItem.Asset is IProjectFileGeneratorAsset projectGeneratorAsset)
                        {
                            try
                            {
                                options.Logger.Info($"Processing: {assetItem}");
                                projectGeneratorAsset.SaveGeneratedAsset(assetItem);
                            }
                            catch (Exception ex)
                            {
                                options.Logger.Error($"Unhandled exception while updating generated files for {assetItem}.", ex);
                            }
                        }
                    }

                    return (int) BuildResultCode.Successful;
                }

                if (unexpectedArgs.Any())
                    throw new OptionException($"Unexpected arguments [{string.Join(", ", unexpectedArgs)}].", nameof(args));

                try
                {
                    options.ValidateOptions();
                }
                catch (ArgumentException ex)
                {
                    throw new OptionException(ex.Message, ex.ParamName);
                }

                if (showHelp)
                {
                    p.WriteOptionDescriptions(Console.Out);
                    return (int)BuildResultCode.Successful;
                }
                else if (packMode)
                {
                    PackageSessionPublicHelper.FindAndSetMSBuildVersion();

                    var logger = new LoggerResult();

                    var csprojFile = options.PackageFile;
                    var intermediatePackagePath = options.BuildDirectory;
                    var generatedItems = new List<(string SourcePath, string PackagePath)>();

                    if (!PackAssetsHelper.Run(logger, csprojFile, intermediatePackagePath, generatedItems))
                    {
                        foreach (var message in logger.Messages)
                        {
                            Console.WriteLine(message);
                        }
                        return (int) BuildResultCode.BuildError;
                    }
                    foreach (var (SourcePath, PackagePath) in generatedItems)
                    {
                        Console.WriteLine($"{SourcePath}|{PackagePath}");
                    }
                    return (int) BuildResultCode.Successful;
                }

                // Also write logs from master process into a file
                if (options.SlavePipe == null)
                {
                    if (options.EnableFileLogging)
                    {
                        string logFileName = options.CustomLogFileName;
                        if (string.IsNullOrEmpty(logFileName))
                        {
                            string inputName = Path.GetFileNameWithoutExtension(options.PackageFile);
                            logFileName = "Logs/Build-" + inputName + "-" + DateTime.Now.ToString("yy-MM-dd-HH-mm") + ".txt";
                        }

                        string dirName = Path.GetDirectoryName(logFileName);
                        if (dirName != null)
                            Directory.CreateDirectory(dirName);

                        fileLogListener = new TextWriterLogListener(new FileStream(logFileName, FileMode.Create)) { TextFormatter = FormatLog };
                        GlobalLogger.GlobalMessageLogged += fileLogListener;
                    }

                    options.Logger.Info("BuildEngine arguments: " + string.Join(" ", args));
                    options.Logger.Info("Starting builder.");
                }
                else
                {
                    IsSlave = true;
                }

                if (!string.IsNullOrEmpty(options.TestName))
                {
                    var test = new TestSession();
                    test.RunTest(options.TestName, options.Logger);
                    exitCode = BuildResultCode.Successful;
                }
                else
                {
                    builder = new PackageBuilder(options);
                    if (!IsSlave)
                    {
                        Console.CancelKeyPress += OnConsoleOnCancelKeyPress;
                    }
                    exitCode = builder.Build();
                }
            }
            catch (OptionException ex)
            {
                options.Logger.Error($"Command option '{ex.OptionName}': {ex.Message}");
                exitCode = BuildResultCode.CommandLineError;
            }
            catch (Exception ex)
            {
                options.Logger.Error($"Unhandled exception.", ex);
                exitCode = BuildResultCode.BuildError;
            }
            finally
            {
                if (fileLogListener != null)
                {
                    GlobalLogger.GlobalMessageLogged -= fileLogListener;
                    fileLogListener.LogWriter.Close();
                }

                // Output logs to the console with colored messages
                if (globalLoggerOnGlobalMessageLogged != null)
                {
                    GlobalLogger.GlobalMessageLogged -= globalLoggerOnGlobalMessageLogged;
                }
                if (builder != null && !IsSlave)
                {
                    Console.CancelKeyPress -= OnConsoleOnCancelKeyPress;
                }

                // Reset cache hold by YamlSerializer
                YamlSerializer.Default.ResetCache();
            }
            return (int) exitCode;
        }

        private void OnConsoleOnCancelKeyPress(object _, ConsoleCancelEventArgs e)
        {
            e.Cancel = builder.Cancel();
        }

        private static string FormatLog(ILogMessage message)
        {
            //$filename($row,$column): $error_type $error_code: $error_message
            //C:\Code\Stride\sources\assets\Stride.Core.Assets.CompilerApp\PackageBuilder.cs(89,13,89,70): warning CS1717: Assignment made to same variable; did you mean to assign something else?

            var builder = new StringBuilder();

            // Location
            if (message is AssetLogMessage assetLogMessage)
                builder.Append($"{assetLogMessage.File}({assetLogMessage.Line + 1},{assetLogMessage.Character + 1}): ");
            // Message type
            builder.Append(message.Type.ToString().ToLowerInvariant()).Append(" ");
            builder.Append((clock.ElapsedMilliseconds * 0.001).ToString("0.000"));
            builder.Append("s: ");
            builder.Append($"[{message.Module ?? "AssetCompiler"}] ");
            builder.Append(message.Text);
            var exceptionInfo = message.ExceptionInfo;
            if (exceptionInfo != null)
            {
                builder.Append(". Exception: ");
                builder.Append(exceptionInfo);
            }
            return builder.ToString();
        }
    }
}
