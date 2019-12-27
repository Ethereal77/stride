// Copyright (c) Xenko contributors (https://xenko.com) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using Xenko.Core.LZ4;

namespace Xenko.Graphics.Regression
{
    public partial class TestRunner
    {
        public const string XenkoVersion = "XENKO_VERSION";

        public const string XenkoBuildNumber = "XENKO_BUILD_NUMBER";

        public const string XenkoTestName = "XENKO_TEST_NAME";

        public const string XenkoBranchName = "XENKO_BRANCH_NAME";
    }

    enum ImageServerMessageType
    {
        ConnectionFinished = 0,
        SendImage = 1,
        RequestImageComparisonStatus = 2,
    }

    public class PlatformPermutator
    {
        public static ImageTestResultConnection GetDefaultImageTestResultConnection()
        {
            var result = new ImageTestResultConnection();

            // TODO: Check build number in environment variables
            result.BuildNumber = -1;

            result.Platform = "Windows";
            result.Serial = Environment.MachineName;
#if XENKO_GRAPHICS_API_DIRECT3D12
            result.DeviceName = "Direct3D12";
#elif XENKO_GRAPHICS_API_DIRECT3D11
            result.DeviceName = "Direct3D";
#endif

            return result;
        }

        public static string GetCurrentPlatformName()
        {
            return GetPlatformName(GetPlatform());
        }

        public static string GetPlatformName(TestPlatform platform)
        {
            switch (platform)
            {
                case TestPlatform.WindowsDx:
                    return "Windows_Direct3D11";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static TestPlatform GetPlatform()
        {
#if XENKO_GRAPHICS_API_NULL
            return TestPlatform.None;
#elif XENKO_GRAPHICS_API_DIRECT3D
            return TestPlatform.WindowsDx;
#endif
        }
    }

    [Flags]
    public enum ImageComparisonFlags
    {
        CopyOnShare = 1,
    }

    public class ImageTestResultConnection
    {
        public int BuildNumber;
        public string Platform;
        public string Serial;
        public string DeviceName;
        public string BranchName = "";
        public ImageComparisonFlags Flags;

        public void Read(BinaryReader reader)
        {
            Platform = reader.ReadString();
            BuildNumber = reader.ReadInt32();
            Serial = reader.ReadString();
            DeviceName = reader.ReadString();
            BranchName = reader.ReadString();
            Flags = (ImageComparisonFlags)reader.ReadInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Platform);
            writer.Write(BuildNumber);
            writer.Write(Serial);
            writer.Write(DeviceName);
            writer.Write(BranchName);
            writer.Write((int)Flags);
        }
    }

    public struct ImageInformation
    {
        public int Width;
        public int Height;
        public int TextureSize;
        public int BaseVersion;
        public int CurrentVersion;
        public int FrameIndex;
        public TestPlatform Platform;
        public PixelFormat Format;
    }

    public enum TestPlatform
    {
        None,
        WindowsDx
    }
}
