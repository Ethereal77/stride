// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Stride.Core.Diagnostics;

namespace Stride.Core.Assets.CompilerApp
{
    public class PackageBuilderOptions
    {
        public readonly LoggerResult Logger;

        public bool Verbose = false;
        public bool Debug = false;

        public List<string> LogPipeNames = new List<string>();
        public List<string> MonitorPipeNames = new List<string>();
        public bool EnableFileLogging;
        public string CustomLogFileName;
        public string SlavePipe;
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
        public Dictionary<string, string> ExtraCompileProperties;
        public int ThreadCount = Environment.ProcessorCount;

        public string TestName;

        public bool DisableAutoCompileProjects { get; set; }
        public string ProjectConfiguration { get; set; }
        public string OutputDirectory { get; set; }
        public string BuildDirectory { get; set; }
        public string SolutionFile { get; set; }
        public Guid PackageId { get; set; }
        public PlatformType Platform { get; set; }
        public string PackageFile { get; set; }
        public string MSBuildUpToDateCheckFileBase { get; set; }


        public PackageBuilderOptions(LoggerResult logger)
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            Logger = logger;
        }

        /// <summary>
        ///   Gets the log message types to log depending on the current options.
        /// </summary>
        public LogMessageType LoggerType => Debug ? LogMessageType.Debug : (Verbose ? LogMessageType.Verbose : LogMessageType.Info);

        /// <summary>
        ///   Determines whether the current builder options are valid for the execution of a slave session.
        /// </summary>
        /// <returns><c>true</c> if the options are valid for a slave session.</returns>
        public bool IsValidForSlave() => !string.IsNullOrEmpty(SlavePipe) &&
                                         !string.IsNullOrEmpty(BuildDirectory);

        /// <summary>
        ///   Validates the options to ensure that every parameter is correct for a master execution.
        /// </summary>
        /// <exception cref="ArgumentException">This tool requires one input file.</exception>
        /// <exception cref="ArgumentException">The given working directory does not exist.</exception>
        /// <exception cref="ArgumentException">This tool requires either a --package-file, or a --solution-file and --package-id..</exception>
        /// <exception cref="ArgumentException">The specified package file doesn't exist.</exception>
        public void ValidateOptions()
        {
            if (string.IsNullOrWhiteSpace(BuildDirectory))
                throw new ArgumentException("This tool requires a build path.");

            try
            {
                BuildDirectory = Path.GetFullPath(BuildDirectory);
            }
            catch (Exception)
            {
                throw new ArgumentException("The provided path is not a valid path name.");
            }

            if (SlavePipe is null)
            {
                if (string.IsNullOrWhiteSpace(PackageFile))
                {
                    if (string.IsNullOrWhiteSpace(SolutionFile) || PackageId == Guid.Empty)
                        throw new ArgumentException("This tool requires either a --package-file, or a --solution-file and --package-id.");
                }
                else if (!File.Exists(PackageFile))
                    throw new ArgumentException($"Package file [{PackageFile}] doesn't exist.");
            }
        }
    }
}
