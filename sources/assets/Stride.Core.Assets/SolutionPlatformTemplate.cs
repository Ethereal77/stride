// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;

using Stride.Core;

namespace Stride.Core.Assets
{
    /// <summary>
    /// Defines a custom project template for a <see cref="SolutionPlatform"/>.
    /// </summary>
    [DataContract("SolutionPlatformTemplate")]
    public class SolutionPlatformTemplate
    {
        public SolutionPlatformTemplate(string templatePath, string displayName)
        {
            TemplatePath = templatePath ?? throw new ArgumentNullException(nameof(templatePath));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }

        /// <summary>
        /// The template path.
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        /// The display name, which will be shown to user when choosing template.
        /// </summary>
        public string DisplayName { get; set; }
    }
}