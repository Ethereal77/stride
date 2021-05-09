// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

namespace Stride.Core.Assets.Compiler
{
    /// <summary>
    /// The class represents the argument of the <see cref="ItemListCompiler.AssetCompiled"/> event raised by the <see cref="ItemListCompiler"/> class.
    /// </summary>
    public class AssetCompiledArgs : EventArgs
    {
        /// <summary>
        /// Constructs an <see cref="AssetCompiledArgs"/> instance.
        /// </summary>
        /// <param name="asset">The asset that has been compiled. Cannot be null.</param>
        /// <param name="result">The result of the asset compilation. Cannot be null.</param>
        public AssetCompiledArgs(AssetItem asset, AssetCompilerResult result)
        {
            if (asset == null) throw new ArgumentNullException("asset");
            if (result == null) throw new ArgumentNullException("result");
            Asset = asset;
            Result = result;
        }

        /// <summary>
        /// The asset item that has just been compiled.
        /// </summary>
        public AssetItem Asset { get; set; }

        /// <summary>
        /// The result of the asset compilation.
        /// </summary>
        public AssetCompilerResult Result { get; set; }
    }
}
