// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

/* THIS CODE IS DISABLED, WE WILL HAVE TO CLEANUP ASSEMBLY DEPENDENCIES

using System;
using System.IO;

using Xunit;

using Xenko.PublicApiCheck;

namespace Xenko.Graphics
{
    // CANNOT WORK INSIDE THE SAME SOLUTION. NEED TO RUN THIS OUTSIDE THE SOLUTION
    [Description("Check public Graphics API consistency between Reference and Direct3D")]
    public class TestGraphicsApi
    {
        public const string Platform = "Windows";
        public const string Target = "Debug";

        private const string PathPattern = @"..\..\..\..\..\..\Build\{0}-{1}-{2}\{3}";

        private static readonly string RootPath = Environment.CurrentDirectory;

        private static readonly string ReferencePath = Path.Combine(RootPath, GraphicsPath("Null"));
        private static readonly string GraphicsDirect3DPath = Path.Combine(RootPath, GraphicsPath("Direct3D"));

        private static string GraphicsPath(string api)
        {
            return string.Format(PathPattern, Platform, api, Target, "Xenko.Graphics.dll");
        }


        [Fact]
        public void TestDirect3D()
        {
            Assert.That(ApiCheck.DiffAssemblyToString(ReferencePath, GraphicsDirect3DPath), Is.Null);
        }
    }
}
*/
