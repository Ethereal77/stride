// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// See the LICENSE.md file in the project root for full license information.

namespace NShader
{
    /// <summary>
    /// Supported extensions. Loaded by NShaderScannerFactory.
    /// WARNING, you need also to add those extensions manually to NShader.cs and NShader.pkgdef!
    /// </summary>
    public class NShaderSupportedExtensions
    {
        // HLSL file extensions
        public const string HLSL_FX = ".fx";
        public const string HLSL_FXH = ".fxh";
        public const string HLSL_HLSL = ".hlsl";
        public const string HLSL_VSH = ".vsh";
        public const string HLSL_PSH = ".psh";
        public const string SL_FX = ".slfx";

        // Stride file extensions
        public const string Stride_Shader = ".sdsl";
        public const string Stride_Effect = ".sdfx";
    }
}
