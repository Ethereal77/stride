// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Translation.Annotations;

namespace Stride.Assets.Presentation.AssetEditors.GameEditor
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
