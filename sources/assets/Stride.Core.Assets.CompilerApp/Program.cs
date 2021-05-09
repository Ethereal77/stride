// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;

namespace Stride.Core.Assets.CompilerApp
{
    class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                // Running first time? If yes, create nuget redirect package.
                //var packageVersion = new PackageVersion(StrideVersion.NuGetVersion);
                //if (PackageStore.Instance.IsDevelopmentStore)
                //{
                //    PackageStore.Instance.CheckDeveloperTargetRedirects("Stride", packageVersion, PackageStore.Instance.InstallationPath).Wait();
                //}

                var packageBuilder = new PackageBuilderApp();
                var returnValue =  packageBuilder.Run(args);

                return returnValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception in AssetCompiler: {0}", ex);
                return 1;
            }
            finally
            {
                // Free all native library loaded from the process
                // We cannot free native libraries are some of them are loaded from static module initializer
                // NativeLibrary.UnLoadAll();
            }
        }
    }
}
