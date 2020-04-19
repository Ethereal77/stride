// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Translation.Annotations;

namespace Xenko.Assets.Presentation.AssetEditors.GameEditor
{
    /// <summary>
    /// Represents the different working spaces during rendering
    /// </summary>
    public enum TransformationSpace
    {
        /// <summary>
        /// The absolute world space.
        /// </summary>
        [Translation("Use world coordinates for transformations")]
        WorldSpace,

        /// <summary>
        /// The space from the object point of view.
        /// </summary>
        [Translation("Use local coordinates for transformations")]
        ObjectSpace,

        /// <summary>
        /// The space from the camera point of view.
        /// </summary>
        [Translation("Use current camera projection coordinates for transformations")]
        ViewSpace,
    }
}
