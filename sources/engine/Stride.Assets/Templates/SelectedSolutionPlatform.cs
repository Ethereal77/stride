// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core.Assets;

namespace Stride.Assets.Templates
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
