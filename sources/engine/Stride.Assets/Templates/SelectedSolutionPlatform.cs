// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Assets;

namespace Xenko.Assets.Templates
{
    /// <summary>
    /// Represents a <see cref="SolutionPlatform"/> with some additional parameters (such as <see cref="SolutionPlatformTemplate"/>), as selected in <see cref="NewGameTemplateGenerator"/> or <see cref="UpdatePlatformsTemplateGenerator"/>.
    /// </summary>
    public struct SelectedSolutionPlatform
    {
        public SelectedSolutionPlatform(SolutionPlatform platform, SolutionPlatformTemplate template)
        {
            Platform = platform;
            Template = template;
        }

        public SolutionPlatform Platform { get; }

        public SolutionPlatformTemplate Template { get; }
    }
}
