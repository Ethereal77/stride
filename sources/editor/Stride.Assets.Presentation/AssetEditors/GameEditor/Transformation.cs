// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Translation.Annotations;

namespace Xenko.Assets.Presentation.AssetEditors.GameEditor
{
    /// <summary>
    /// Enumerates the different type of transformation that can be performed on an object.
    /// </summary>
    public enum Transformation
    {
        /// <summary>
        /// A simple translation
        /// </summary>
        [Translation("Use translation gizmo")]
        Translation,

        /// <summary>
        /// A simple rotation
        /// </summary>
        [Translation("Use rotation gizmo")]
        Rotation,

        /// <summary>
        /// A simple scale
        /// </summary>
        [Translation("Use scale gizmo")]
        Scale,
    }
}
