// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Threading.Tasks;

namespace Stride.Core.Assets.Templates
{
    public delegate bool RunGeneratorDelegate();

    /// <summary>
    ///   Provides methods to create a project or package from a template and decide whether a specific template
    ///   matches a description.
    /// </summary>
    public interface ITemplateGenerator
    {
        /// <summary>
        ///   Determines whether this generator is supporting the specified template description.
        /// </summary>
        /// <param name="templateDescription">The template description.</param>
        /// <returns><c>true</c> if this generator is supporting the specified template; otherwise, <c>false</c>.</returns>
        bool IsSupportingTemplate(TemplateDescription templateDescription);
    }

    /// <summary>
    ///   Provides methods to create a project or package from a template.
    /// </summary>
    /// <typeparam name="TParameters">The type of parameters this generator uses.</typeparam>
    public interface ITemplateGenerator<in TParameters> : ITemplateGenerator
        where TParameters : TemplateGeneratorParameters
    {
        /// <summary>
        ///   Prepares this generator with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters for the template generator.</param>
        /// <remarks>
        ///   This method should be used to verify that the parameters are correct, and to ask user for additional
        ///   information before running the template.
        /// </remarks>
        /// <returns>
        ///   A <see cref="Task"/> completing when the preparation is finished, with the result <c>true</c> if the preparation
        ///   was successful, or <c>false</c> otherwise.
        /// </returns>
        Task<bool> PrepareForRun(TParameters parameters);

        /// <summary>
        ///   Runs the generator with the given parameters.
        /// </summary>
        /// <param name="parameters">The parameters for the template generator.</param>
        /// <remarks>
        ///   This method should work in unattended mode and should not ask user for information anymore.
        /// </remarks>
        /// <returns><c>true</c> if the generation was successful; <c>false</c> otherwise.</returns>
        bool Run(TParameters parameters);
    }
}
