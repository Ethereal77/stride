// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.IO;

using Stride.Core;

namespace Stride.Graphics.Regression
{
    public partial class TestRunner
    {
        public const string StrideVersion = "STRIDE_VERSION";
        public const string StrideBuildNumber = "STRIDE_BUILD_NUMBER";
        public const string StrideTestName = "STRIDE_TEST_NAME";
        public const string StrideBranchName = "STRIDE_BRANCH_NAME";
    }

    enum ImageServerMessageType
    {
        ConnectionFinished = 0,
        SendImage = 1,
        RequestImageComparisonStatus = 2
    }

    public static class PlatformPermutator
    {
        public static ImageTestResultConnection GetDefaultImageTestResultConnection()
        {
            var result = new ImageTestResultConnection();

            // TODO: Check build number in environment variables
            result.BuildNumber = -1;

            result.Platform = Platform.Type.ToString();
            result.Serial = Environment.MachineName;
#if STRIDE_GRAPHICS_API_DIRECT3D12
            result.DeviceName = "Direct3D12";
#elif STRIDE_GRAPHICS_API_DIRECT3D11
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
#if STRIDE_GRAPHICS_API_NULL
            return TestPlatform.None;
#elif STRIDE_GRAPHICS_API_DIRECT3D
            return TestPlatform.WindowsDx;
#endif
        }
    }

    [Flags]
    public enum ImageComparisonFlags
    {
        CopyOnShare = 1
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
