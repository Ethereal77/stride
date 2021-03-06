// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Threading;

using Microsoft.Build.Locator;

namespace Stride.Core.Assets
{
    /// <summary>
    ///   Helper class to load / save a Visual Studio solution.
    /// </summary>
    public static class PackageSessionPublicHelper
    {
        private static readonly string[] s_msBuildAssemblies =
        {
            "Microsoft.Build",
            "Microsoft.Build.Framework",
            "Microsoft.Build.Tasks.Core",
            "Microsoft.Build.Utilities.Core"
        };

        private static int MSBuildLocatorCount = 0;
        private static VisualStudioInstance MSBuildInstance;

        /// <summary>
        ///   Finds a compatible version of MSBuild.
        /// </summary>
        public static void FindAndSetMSBuildVersion()
        {
            // Note: this should be called only once
            if (MSBuildInstance is null && Interlocked.Increment(ref MSBuildLocatorCount) == 1)
            {
                // Detect either .NET Core SDK or Visual Studio depending on current runtime
                var isNETCore = !RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework");
                MSBuildInstance = MSBuildLocator.QueryVisualStudioInstances().FirstOrDefault(x => isNETCore
                    ? x.DiscoveryType == DiscoveryType.DotNetSdk && x.Version.Major >= 3
                    : (x.DiscoveryType == DiscoveryType.VisualStudioSetup ||
                       x.DiscoveryType == DiscoveryType.DeveloperConsole) && x.Version.Major >= 16);

                // Make sure it is not already loaded (otherwise MSBuildLocator.RegisterDefaults() throws an exception)
                if (MSBuildInstance != null && !AppDomain.CurrentDomain.GetAssemblies().Any(IsMSBuildAssembly))
                {
                    // We can't use directly RegisterInstance because we want to avoid NuGet verison conflicts (between MSBuild/dotnet one and ours).
                    //   More details at https://github.com/microsoft/MSBuildLocator/issues/127
                    //   This code should be equivalent to MSBuildLocator.RegisterInstance(MSBuildInstance);
                    //   except that we load everything in another context.

                    ApplyDotNetSdkEnvironmentVariables(MSBuildInstance.MSBuildPath);

                    var msbuildAssemblyLoadContext = new AssemblyLoadContext("MSBuild");

                    AssemblyLoadContext.Default.Resolving += (assemblyLoadContext, assemblyName) =>
                    {
                        string path = Path.Combine(MSBuildInstance.MSBuildPath, assemblyName.Name + ".dll");
                        if (File.Exists(path))
                        {
                            return msbuildAssemblyLoadContext.LoadFromAssemblyPath(path);
                        }

                        return null;
                    };
                }
            }

            if (MSBuildInstance is null)
                throw new InvalidOperationException("Could not find a MSBuild installation (expected 16.0 or later).");

            CheckMSBuildToolset();

            // Reset MSBUILD_EXE_PATH once MSBuild is resolved, to not spook child process
            // (had issues with ThisProcess(MSBuild)->CompilerApp(net472): CompilerApp couldn't load MSBuild project properly)
            Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", null);
        }

        // Function copied from MSBuildLocator.ApplyDotNetSdkEnvironmentVariables
		// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
        private static void ApplyDotNetSdkEnvironmentVariables(string dotNetSdkPath)
        {
            const string MSBUILD_EXE_PATH = nameof(MSBUILD_EXE_PATH);
            const string MSBuildExtensionsPath = nameof(MSBuildExtensionsPath);
            const string MSBuildSDKsPath = nameof(MSBuildSDKsPath);

            var variables = new Dictionary<string, string>
            {
                [MSBUILD_EXE_PATH] = dotNetSdkPath + "MSBuild.dll",
                [MSBuildExtensionsPath] = dotNetSdkPath,
                [MSBuildSDKsPath] = dotNetSdkPath + "Sdks"
            };

            foreach (var kvp in variables)
            {
                Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
            }
        }

        private static bool IsMSBuildAssembly(Assembly assembly)
        {
            return IsMSBuildAssembly(assembly.GetName());
        }

        private static bool IsMSBuildAssembly(AssemblyName assemblyName)
        {
            return s_msBuildAssemblies.Contains(assemblyName.Name, StringComparer.OrdinalIgnoreCase);
        }

        private static void CheckMSBuildToolset()
        {
            // Check that we can create a project
            using (var projectCollection = new Microsoft.Build.Evaluation.ProjectCollection())
            {
                // VS 2019+ (https://github.com/Microsoft/msbuild/issues/3778)
                if (projectCollection.GetToolset("Current") is null)
                {
                    throw new InvalidOperationException("Could not find a supported MSBuild toolset version (expected 16.0 or later).");
                }
            }
        }
    }
}
