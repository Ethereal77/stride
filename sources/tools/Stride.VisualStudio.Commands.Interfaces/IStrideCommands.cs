// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// See the LICENSE.md file in the project root for full license information.

using Stride.VisualStudio.Commands.Shaders;

namespace Stride.VisualStudio.Commands
{
    /// <summary>
    ///   Defines Stride commands that can be accessed by VS Package for the current Stride package
    ///   (so that VSPackage doesn't depend on Stride assemblies).
    /// </summary>
    /// <remarks>
    ///   Removing any of those methods will likely break backwards compatibility.
    /// </remarks>
    public interface IStrideCommands
    {
        byte[] GenerateShaderKeys(string inputFileName, string inputFileContent);

        RawShaderNavigationResult AnalyzeAndGoToDefinition(string projectPath, string sourceCode, RawSourceSpan span);
    }
}
