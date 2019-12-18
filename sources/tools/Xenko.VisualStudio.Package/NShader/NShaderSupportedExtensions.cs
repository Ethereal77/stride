#region Header Licence
//  ---------------------------------------------------------------------
//
//  Copyright (c) 2009 Alexandre Mutel and Microsoft Corporation.
//  All rights reserved.
//
//  This code module is part of NShader, a plugin for visual studio
//  to provide syntax highlighting for shader languages (HLSL, XKSL)
//
//  ------------------------------------------------------------------
//
//  This code is licensed under the Microsoft Public License.
//  See the file License.txt for the license details.
//  More info on: http://nshader.codeplex.com
//
//  ------------------------------------------------------------------
#endregion
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
