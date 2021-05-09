// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.Setup.Configuration;

namespace Stride.Core.Tasks
{
    public class LocateDevenv : Task
    {
        [Output]
        public string DevenvPath { get; set; }

        public override bool Execute()
        {
            DevenvPath = FindDevenv(msbuildPath: null);

            return DevenvPath is not null;
        }

        internal static string FindDevenv(string msbuildPath)
        {
            try
            {
                // Use Microsoft.VisualStudio.Setup.Configuration.Interop (works when running from Visual Studio)
                var setupConfiguration = new SetupConfiguration() as ISetupConfiguration;
                ISetupInstance instanceForCurrentProcess = !string.IsNullOrEmpty(msbuildPath)
                    ? setupConfiguration.GetInstanceForPath(msbuildPath)
                    : setupConfiguration.GetInstanceForCurrentProcess(); // Works when ran as MSBuild Task only

                return Path.Combine(instanceForCurrentProcess.GetInstallationPath(), @"Common7\IDE\devenv.exe");
            }
            catch
            {
                return null;
            }
        }
    }
}
