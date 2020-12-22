// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Stride.Core;
using Stride.Core.Assets;
using Stride.Core.VisualStudio;

namespace Stride.Assets
{
    [DataContract("Stride")]
    public sealed class StrideConfig
    {
        public const string PackageName = "Stride";

        public static readonly PackageVersion LatestPackageVersion = new PackageVersion(StrideVersion.NuGetVersion);

        private static readonly string ProgramFilesX86 = Environment.GetEnvironmentVariable(Environment.Is64BitOperatingSystem ? "ProgramFiles(x86)" : "ProgramFiles");

        private static readonly Version VS2015Version = new Version(14, 0);
        private static readonly Version VSAnyVersion = new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);

        public static PackageDependency GetLatestPackageDependency()
        {
            return new PackageDependency(PackageName, new PackageVersionRange()
                {
                    MinVersion = LatestPackageVersion,
                    IsMinInclusive = true
                });
        }

        /// <summary>
        ///   Registers the solution platforms supported by Stride.
        /// </summary>
        internal static void RegisterSolutionPlatforms()
        {
            AssetRegistry.RegisterSupportedPlatforms(new List<SolutionPlatform>
            {
                // Windows
                new SolutionPlatform()
                {
                    Name = PlatformType.Windows.ToString(),
                    IsAvailable = true,
                    TargetFramework = "net5.0",
                    RuntimeIdentifier = "win-x64",
                    Type = PlatformType.Windows
                }
            });
        }

        /// <summary>
        ///   Checks if any of the provided component versions are available on the system.
        /// </summary>
        /// <param name="vsVersionToComponent">A dictionary of Visual Studio versions to their respective paths for a given component.</param>
        /// <returns><c>true</c> if any of the components in the dictionary are available, <c>false</c> otherwise.</returns>
        internal static bool IsVSComponentAvailableAnyVersion(IDictionary<Version, string> vsVersionToComponent)
        {
            if (vsVersionToComponent is null)
                throw new ArgumentNullException(nameof(vsVersionToComponent));

            foreach (var pair in vsVersionToComponent)
            {
                if (pair.Key == VS2015Version)
                {
                    return IsFileInProgramFilesx86Exist(pair.Value);
                }
                else
                {
                    return VisualStudioVersions.AvailableVisualStudioInstances.Any(
                        ideInfo => ideInfo.PackageVersions.ContainsKey(pair.Value));
                }
            }
            return false;
        }

        /// <summary>
        ///   Checks if a particular component set is available for the specified IDE version.
        /// </summary>
        /// <param name="ideInfo">The IDE info to search for the components.</param>
        /// <param name="vsVersionToComponent">A dictionary of Visual Studio versions to their respective paths for a given component.</param>
        /// <returns><c>true</c> if any of the components in the dictionary are available, <c>false</c> otherwise.</returns>
        internal static bool IsVSComponentAvailableForIDE(IDEInfo ideInfo, IDictionary<Version, string> vsVersionToComponent)
        {
            if (ideInfo is null)
                throw new ArgumentNullException(nameof(ideInfo));
            if (vsVersionToComponent is null)
                throw new ArgumentNullException(nameof(vsVersionToComponent));

            if (vsVersionToComponent.TryGetValue(ideInfo.Version, out string path))
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
            return ProgramFilesX86 != null && File.Exists(Path.Combine(ProgramFilesX86, path));
        }
    }
}
