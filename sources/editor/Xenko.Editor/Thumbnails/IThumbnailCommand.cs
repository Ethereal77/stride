// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Diagnostics;

namespace Xenko.Editor.Thumbnails
{
    public interface IThumbnailCommand
    {
        /// <summary>
        /// Gets or sets the dependency build step.
        /// </summary>
        /// <value>
        /// The dependency build step.
        /// </value>
        LogMessageType DependencyBuildStatus { get; set; }
    }
}
