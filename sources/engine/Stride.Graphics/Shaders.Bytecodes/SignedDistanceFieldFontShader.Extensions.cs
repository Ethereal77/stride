// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Shaders;

namespace Stride.Graphics
{
    internal partial class SignedDistanceFieldFontShader
    {
        private static EffectBytecode bytecode;

        internal static EffectBytecode Bytecode
        {
            get
            {
                return bytecode ?? (bytecode = EffectBytecode.FromBytesSafe(signedDistanceFieldFontBytecode));
            }
        }
    }
}
