// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Assets.Compiler
{
    /// <summary>
    /// Interface for compiling a package.
    /// </summary>
    public interface IPackageCompiler
    {
        /// <summary>
        /// Prepares a package with the specified compiler context.
        /// </summary>
        /// <param name="compilerContext">The compiler context.</param>
        /// <returns>Result of compilation.</returns>
        AssetCompilerResult Prepare(AssetCompilerContext compilerContext);
    }
}
