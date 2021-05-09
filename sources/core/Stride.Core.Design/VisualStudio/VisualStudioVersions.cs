// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.Setup.Configuration;

namespace Stride.Core.VisualStudio
{
    public class IDEInfo
    {
        public bool IsComplete { get; }

        public string DisplayName { get; }

        public Version Version { get; }

        /// <summary>
        ///   Gets the path to the build tools of this IDE.
        /// </summary>
        /// <value>The path to the build tools of this IDE, or <c>null</c>.</value>
        public string BuildToolsPath { get; internal set; }

        /// <summary>
        ///   Gets the path to the development environment executable of this IDE.
        /// </summary>
        /// <value>The path to the development environment executable of this IDE, or <c>null</c>.</value>
        public string DevenvPath { get; internal set; }

        /// <summary>
        ///   Gets the root installation path of this IDE.
        /// </summary>
        /// <value>The root installation path of this IDE. This can be empty but not <c>null</c>.</value>
        public string InstallationPath { get; }

        /// <summary>
        ///   Gets the path to the VSIX installer of this IDE.
        /// </summary>
        /// <value>The path to the VSIX installer of this IDE, or <c>null</c>.</value>
        public string VsixInstallerPath { get; internal set; }

        public VSIXInstallerVersion VsixInstallerVersion { get; internal set; }

        public Dictionary<string, string> PackageVersions { get; } = new Dictionary<string, string>();

        /// <summary>
        ///   Gets a value indicating whether this IDE has integrated build tools.
        /// </summary>
        public bool HasBuildTools => !string.IsNullOrEmpty(BuildToolsPath);

        /// <summary>
        ///   Gets a value indicating whether this IDE has a development environment.
        /// </summary>
        public bool HasDevenv => !string.IsNullOrEmpty(DevenvPath);

        /// <summary>
        ///   Gets a value indicating whether this IDE has a VSIX installer.
        /// </summary>
        public bool HasVsixInstaller => !string.IsNullOrEmpty(VsixInstallerPath) && VsixInstallerVersion != VSIXInstallerVersion.None;


        public IDEInfo(Version version, string displayName, string installationPath, bool complete = true)
        {
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            IsComplete = complete;
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Version = version;
            InstallationPath = installationPath ?? throw new ArgumentNullException(nameof(installationPath));
        }


        /// <inheritdoc />
        public override string ToString() => DisplayName;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum VSIXInstallerVersion
    {
        None,
        VS2019AndFutureVersions
    }

    public static class VisualStudioVersions
    {
        // ReSharper disable once InconsistentNaming
        private const int REGDB_E_CLASSNOTREG = unchecked((int) 0x80040154);
        private static Lazy<List<IDEInfo>> IDEInfos = new Lazy<List<IDEInfo>>(BuildIDEInfos);

        public static IDEInfo DefaultIDE = new IDEInfo(new Version("0.0"), "Default IDE", string.Empty);

        /// <summary>
        ///   Gets a list of the available instances of Visual Studio 2019 or later versions.
        /// </summary>
        /// <remarks>Previous versions are not supported due to lack of <c>buildTransitive</c> targets.</remarks>
        public static IEnumerable<IDEInfo> AvailableVisualStudioInstances => IDEInfos.Value.Where(x => x.Version.Major >= 16 && x.HasDevenv);

        /// <summary>
        ///   Gets a list of the available compatible instances of Visual Studio.
        /// </summary>
        /// <remarks>Previous versions are not supported due to lack of <c>buildTransitive</c> targets.</remarks>
        public static IEnumerable<IDEInfo> AllAvailableVisualStudioInstances => IDEInfos.Value.Where(x => x.Version.Major >= 16 && x.HasDevenv);

        public static IEnumerable<IDEInfo> AvailableBuildTools => IDEInfos.Value.Where(x => x.HasBuildTools);

        public static void Refresh()
        {
            IDEInfos = new Lazy<List<IDEInfo>>(BuildIDEInfos);
        }

        private static List<IDEInfo> BuildIDEInfos()
        {
            var ideInfos = new List<IDEInfo>();

            // Visual Studio 16.0 (2019) and later
            try
            {
                var configuration = new SetupConfiguration();

                var instances = configuration.EnumAllInstances();
                instances.Reset();
                var vsInstance = new ISetupInstance[1];

                while (true)
                {
                    instances.Next(1, vsInstance, out int pceltFetched);
                    if (pceltFetched <= 0)
                        break;

                    try
                    {
                        if (!(vsInstance[0] is ISetupInstance2 setupInstance))
                            continue;

                        // Only deal with VS2019+
                        if (!Version.TryParse(setupInstance.GetInstallationVersion(), out var version) || version.Major < 16)
                            continue;

                        var installationPath = setupInstance.GetInstallationPath();
                        var buildToolsPath = Path.Combine(installationPath, "MSBuild", "Current", "Bin");
                        if (!Directory.Exists(buildToolsPath))
                            buildToolsPath = null;
                        var idePath = Path.Combine(installationPath, "Common7", "IDE");
                        var devenvPath = Path.Combine(idePath, "devenv.exe");
                        if (!File.Exists(devenvPath))
                            devenvPath = null;
                        var vsixInstallerPath = Path.Combine(idePath, "VSIXInstaller.exe");
                        if (!File.Exists(vsixInstallerPath))
                            vsixInstallerPath = null;

                        var displayName = setupInstance.GetDisplayName();
                        // Try to append nickname (if any)
                        try
                        {
                            var nickname = setupInstance.GetProperties().GetValue("nickname") as string;
                            if (!string.IsNullOrEmpty(nickname))
                                displayName = $"{displayName} ({nickname})";
                            else
                            {
                                var installationName = setupInstance.GetInstallationName();
                                // In case of a Preview version, we have:
                                //   "installationName": "VisualStudioPreview/16.4.0-pre.6.0+29519.161"
                                //   "channelId": "VisualStudio.16.Preview"
                                if (installationName.Contains("Preview"))
                                {
                                    displayName += " (Preview)";
                                }
                            }
                        }
                        catch (COMException) { }

                        try
                        {
                            var minimumRequiredState = InstanceState.Local | InstanceState.Registered;
                            if ((setupInstance.GetState() & minimumRequiredState) != minimumRequiredState)
                                continue;
                        }
                        catch (COMException)
                        {
                            continue;
                        }

                        var ideInfo = new IDEInfo(version, displayName, installationPath, setupInstance.IsComplete())
                        {
                            BuildToolsPath = buildToolsPath,
                            DevenvPath = devenvPath,
                            VsixInstallerVersion = VSIXInstallerVersion.VS2019AndFutureVersions,
                            VsixInstallerPath = vsixInstallerPath,
                        };

                        // Fill packages
                        foreach (var package in setupInstance.GetPackages())
                        {
                            ideInfo.PackageVersions[package.GetId()] = package.GetVersion();
                        }

                        ideInfos.Add(ideInfo);
                    }
                    catch
                    {
                        // Something might have happened inside Visual Studio Setup code (f.e, FileNotFoundException in GetInstallationPath())
                        // Let's ignore this instance
                    }
                }
            }
            catch (COMException comException) when (comException.HResult == REGDB_E_CLASSNOTREG)
            {
                // COM is not registered. Assuming no instances are installed.
            }

            return ideInfos;
        }
    }
}
