// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Collections.Generic;

namespace Stride.Core.Assets.Compiler
{
    /// <summary>
    /// Enumerate assets that <see cref="PackageCompiler"/> will process.
    /// </summary>
    public interface IPackageCompilerSource
    {
        /// <summary>
        /// Enumerates assets.
        /// </summary>
        IEnumerable<AssetItem> GetAssets(AssetCompilerResult assetCompilerResult);
    }
}
