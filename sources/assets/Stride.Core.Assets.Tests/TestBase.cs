// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;

using Xunit;

namespace Stride.Core.Assets.Tests
{
    public class TestBase
    {
        public readonly string DirectoryTestBase = Path.Combine(AssemblyDirectory, @"data\");

        public static void GenerateAndCompare(string title, string outputFilePath, string referenceFilePath, Asset asset)
        {
            Console.WriteLine(title + @"- from file " + outputFilePath);
            Console.WriteLine(@"---------------------------------------");
            AssetFileSerializer.Save(outputFilePath, asset, null);
            var left = File.ReadAllText(outputFilePath).Trim();
            Console.WriteLine(left);
            var right = File.ReadAllText(referenceFilePath).Trim();
            Assert.Equal(right, left);
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

    }
}
