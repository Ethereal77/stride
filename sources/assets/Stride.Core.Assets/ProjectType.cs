// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Core.Assets
{
    // NOTE: Beware of the order of values in this enum, it is used for sorting.

    /// <summary>
    ///   Type of the project.
    /// </summary>
    [DataContract("ProjectType")]
    public enum ProjectType
    {
        /// <summary>
        ///   The project is compiled as a library.
        /// </summary>
        Library,

        /// <summary>
        ///   The project is compiled as an executable.
        /// </summary>
        Executable
    }
}
