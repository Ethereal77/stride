// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;

using Xenko.Core.IO;

namespace Xenko.Assets.Presentation.AssetEditors
{
    /// <summary>
    /// Provides information about scripts in loaded assemblies.
    /// </summary>
    public interface IScriptSourceCodeResolver
    {
        /// <summary>
        /// Gets script types that exist in a source file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypesFromSourceFile(UFile file);

        Compilation LatestCompilation { get; }

        event EventHandler LatestCompilationChanged;
    }
}
