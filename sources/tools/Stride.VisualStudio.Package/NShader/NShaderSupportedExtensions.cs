// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2009 NShader - Alexandre Mutel, Microsoft Corporation
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

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

        // Xenko file extensions
        public const string Xenko_Shader = ".xksl";
        public const string Xenko_Effect = ".xkfx";
    }
}
