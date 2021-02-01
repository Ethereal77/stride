// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Core.Packages
{
    /// <summary>
    ///   Defines the actions that can be in progress when operating on a <see cref="NugetPackage"/>.
    /// </summary>
    public enum ProgressAction
    {
        /// <summary>
        ///   A specific version of the package is being downloaded.
        /// </summary>
        Download,

        /// <summary>
        ///   A specific version of the package is being installed.
        /// </summary>
        Install,

        /// <summary>
        ///   A specific version of the package is being removed.
        /// </summary>
        Delete,
    }
}
