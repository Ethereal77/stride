// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xenko.Core.Assets;
using Xenko.Core;
using Xenko.Core.VisualStudio;

namespace Xenko.Assets
{
    [DataContract("Xenko")]
    public sealed class XenkoConfig
    {
        public const string PackageName = "Xenko";

        public static readonly PackageVersion LatestPackageVersion = new PackageVersion(XenkoVersion.NuGetVersion);

        private static readonly string ProgramFilesX86 = Environment.GetEnvironmentVariable(Environment.Is64BitOperatingSystem ? "ProgramFiles(x86)" : "ProgramFiles");

        private static readonly Version VS2015Version = new Version(14, 0);
        private static readonly Version VSAnyVersion = new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);

        internal static readonly Dictionary<Version, string> UniversalWindowsPlatformComponents = new Dictionary<Version, string>
        {
            { VSAnyVersion, @"Microsoft.VisualStudio.Component.UWP.Support" },
            { VS2015Version, @"MSBuild\Microsoft\WindowsXaml\v14.0\8.2\Microsoft.Windows.UI.Xaml.Common.Targets" }
        };

        public static PackageDependency GetLatestPackageDependency()
        {
            return new PackageDependency(PackageName, new PackageVersionRange()
                {
                    MinVersion = LatestPackageVersion,
                    IsMinInclusive = true
                });
        }

        /// <summary>
        /// Registers the solution platforms supported by Xenko.
        /// </summary>
        internal static void RegisterSolutionPlatforms()
        {
            var solutionPlatforms = new List<SolutionPlatform>();

            // Windows
            var windowsPlatform = new SolutionPlatform()
                {
                    Name = PlatformType.Windows.ToString(),
                    IsAvailable = true,
                    Alias = "Any CPU",
                    TargetFramework = "net461",
                    Type = PlatformType.Windows
                };
            windowsPlatform.PlatformsPart.Add(new SolutionPlatformPart("Any CPU"));
            windowsPlatform.PlatformsPart.Add(new SolutionPlatformPart("Mixed Platforms") { Alias = "Any CPU"});
            windowsPlatform.DefineConstants.Add("XENKO_PLATFORM_WINDOWS");
            windowsPlatform.DefineConstants.Add("XENKO_PLATFORM_WINDOWS_DESKTOP");
            windowsPlatform.Configurations.Add(new SolutionConfiguration("Testing"));
            windowsPlatform.Configurations.Add(new SolutionConfiguration("AppStore"));

            // Currently disabled
            //windowsPlatform.Configurations.Add(coreClrDebug);
            //windowsPlatform.Configurations.Add(coreClrRelease);
            foreach (var part in windowsPlatform.PlatformsPart)
            {
                part.Configurations.Clear();
                part.Configurations.AddRange(windowsPlatform.Configurations);
            }
            solutionPlatforms.Add(windowsPlatform);

            // Universal Windows Platform (UWP)
            var uwpPlatform = new SolutionPlatform()
            {
                Name = PlatformType.UWP.ToString(),
                Type = PlatformType.UWP,
                TargetFramework = "uap10.0.16299",
                Templates =
                {
                    //new SolutionPlatformTemplate("ProjectExecutable.UWP/CoreWindow/ProjectExecutable.UWP.ttproj", "Core Window"),
                    new SolutionPlatformTemplate("ProjectExecutable.UWP/Xaml/ProjectExecutable.UWP.ttproj", "Xaml")
                },
                IsAvailable = IsVSComponentAvailableAnyVersion(UniversalWindowsPlatformComponents),
                UseWithExecutables = false,
                IncludeInSolution = false,
            };

            uwpPlatform.DefineConstants.Add("XENKO_PLATFORM_WINDOWS");
            uwpPlatform.DefineConstants.Add("XENKO_PLATFORM_UWP");
            uwpPlatform.Configurations.Add(new SolutionConfiguration("Testing"));
            uwpPlatform.Configurations.Add(new SolutionConfiguration("AppStore"));
            uwpPlatform.Configurations["Release"].Properties.Add("<NoWarn>;2008</NoWarn>");
            uwpPlatform.Configurations["Debug"].Properties.Add("<NoWarn>;2008</NoWarn>");
            uwpPlatform.Configurations["Testing"].Properties.Add("<NoWarn>;2008</NoWarn>");
            uwpPlatform.Configurations["AppStore"].Properties.Add("<NoWarn>;2008</NoWarn>");

            uwpPlatform.Configurations["Release"].Properties.Add("<UseDotNetNativeToolchain>true</UseDotNetNativeToolchain>");
            uwpPlatform.Configurations["Testing"].Properties.Add("<UseDotNetNativeToolchain>true</UseDotNetNativeToolchain>");
            uwpPlatform.Configurations["AppStore"].Properties.Add("<UseDotNetNativeToolchain>true</UseDotNetNativeToolchain>");

            foreach (var cpu in new[] { "x86", "x64", "ARM" })
            {
                var uwpPlatformCpu = new SolutionPlatformPart(uwpPlatform.Name + "-" + cpu)
                {
                    LibraryProjectName = uwpPlatform.Name,
                    ExecutableProjectName = cpu,
                    Cpu = cpu,
                    InheritConfigurations = true,
                    UseWithLibraries = false,
                    UseWithExecutables = true,
                };
                uwpPlatformCpu.Configurations.Clear();
                uwpPlatformCpu.Configurations.AddRange(uwpPlatform.Configurations);

                uwpPlatform.PlatformsPart.Add(uwpPlatformCpu);
            }

            solutionPlatforms.Add(uwpPlatform);

            // Linux
            var linuxPlatform = new SolutionPlatform()
            {
                Name = PlatformType.Linux.ToString(),
                IsAvailable = true,
                TargetFramework = "netcoreapp2.1",
                RuntimeIdentifier = "linux-x64",
                Type = PlatformType.Linux,
            };
            linuxPlatform.DefineConstants.Add("XENKO_PLATFORM_UNIX");
            linuxPlatform.DefineConstants.Add("XENKO_PLATFORM_LINUX");
            solutionPlatforms.Add(linuxPlatform);

            AssetRegistry.RegisterSupportedPlatforms(solutionPlatforms);
        }

        /// <summary>
        /// Checks if any of the provided component versions are available on this system
        /// </summary>
        /// <param name="vsVersionToComponent">A dictionary of Visual Studio versions to their respective paths for a given component</param>
        /// <returns>true if any of the components in the dictionary are available, false otherwise</returns>
        internal static bool IsVSComponentAvailableAnyVersion(IDictionary<Version, string> vsVersionToComponent)
        {
            if (vsVersionToComponent == null) { throw new ArgumentNullException("vsVersionToComponent"); }

            foreach (var pair in vsVersionToComponent)
            {
                if (pair.Key == VS2015Version)
                {
                    return IsFileInProgramFilesx86Exist(pair.Value);
                }
                else
                {
                    return VisualStudioVersions.AvailableVisualStudioInstances.Any(
                        ideInfo => ideInfo.PackageVersions.ContainsKey(pair.Value)
                    );
                }
            }
            return false;
        }

        /// <summary>
        /// Check if a particular component set for this IDE version
        /// </summary>
        /// <param name="ideInfo">The IDE info to search for the components</param>
        /// <param name="vsVersionToComponent">A dictionary of Visual Studio versions to their respective paths for a given component</param>
        /// <returns>true if the IDE has any of the component versions available, false otherwise</returns>
        internal static bool IsVSComponentAvailableForIDE(IDEInfo ideInfo, IDictionary<Version, string> vsVersionToComponent)
        {
            if (ideInfo == null) { throw new ArgumentNullException("ideInfo"); }
            if (vsVersionToComponent == null) { throw new ArgumentNullException("vsVersionToComponent"); }

            string path = null;
            if (vsVersionToComponent.TryGetValue(ideInfo.Version, out path))
            {
                if (ideInfo.Version == VS2015Version)
                {
                    return IsFileInProgramFilesx86Exist(path);
                }
                else
                {
                    return ideInfo.PackageVersions.ContainsKey(path);
                }
            }
            else if (vsVersionToComponent.TryGetValue(VSAnyVersion, out path))
            {
                return ideInfo.PackageVersions.ContainsKey(path);
            }
            return false;
        }

        // For VS 2015
        internal static bool IsFileInProgramFilesx86Exist(string path)
        {
            return (ProgramFilesX86 != null && File.Exists(Path.Combine(ProgramFilesX86, path)));
        }
    }
}
