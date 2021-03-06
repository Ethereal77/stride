// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.UI
{
    /// <summary>
    /// Describes how a child element is positioned in depth or stretched within a parent's layout slot.
    /// </summary>
    public enum DepthAlignment
    {
        /// <summary>
        /// The child element is aligned to the front of the parent's layout slot.
        /// </summary>
        /// <userdoc>The child element is aligned to the front of the parent's layout slot.</userdoc>
        Front,
        /// <summary>
        /// The child element is aligned to the center of the parent's layout slot.
        /// </summary>
        /// <userdoc>The child element is aligned to the center of the parent's layout slot.</userdoc>
        Center,
        /// <summary>
        /// The child element is aligned to the back of the parent's layout slot.
        /// </summary>
        /// <userdoc>The child element is aligned to the back of the parent's layout slot.</userdoc>
        Back,
        /// <summary>
        /// The child element stretches to fill the parent's layout slot.
        /// </summary>
        /// <userdoc>The child element stretches to fill the parent's layout slot.</userdoc>
        Stretch,
    }
}
