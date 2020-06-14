// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.UI
{
    /// <summary>
    ///   Defines the different types of strip possible on a <see cref="Panels.Grid"/>.
    /// </summary>
    public enum StripType
    {
        /// <summary>
        ///   A strip having fixed size expressed in number of virtual pixels.
        /// </summary>
        /// <userdoc>A strip having fixed size expressed in number of virtual pixels.</userdoc>
        Fixed,

        /// <summary>
        ///   A strip that occupies exactly the size required by its content.
        /// </summary>
        /// <userdoc>A strip that occupies exactly the size required by its content. </userdoc>
        Auto,

        /// <summary>
        ///   A strip that occupies the maximum available size, dispatched among the other star-sized columns.
        /// </summary>
        /// <userdoc>A strip that occupies the maximum available size, dispatched among the other star-sized columns.</userdoc>
        Star
    }
}
