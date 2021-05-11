// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.UI
{
    /// <summary>
    /// Describes how content is resized to fill its allocated space.
    /// </summary>
    public enum StretchType
    {
        /// <summary>
        /// The content preserves its original size.
        /// </summary>
        /// <userdoc>The content preserves its original size.</userdoc>
        None,
        /// <summary>
        /// The content is resized to fit in the destination dimensions while it preserves its native aspect ratio.
        /// </summary>
        /// <userdoc>The content is resized to fit in the destination dimensions while it preserves its native aspect ratio.</userdoc>
        Uniform,
        /// <summary>
        /// The content is resized to fill the destination dimensions while it preserves its native aspect ratio. 
        /// If the aspect ratio of the destination rectangle differs from the source, the source content is clipped to fit in the destination dimensions.
        /// </summary>
        /// <userdoc>The content is resized to fill the destination dimensions while it preserves its native aspect ratio.
        /// If the aspect ratio of the destination rectangle differs from the source, the source content is clipped to fit in the destination dimensions.</userdoc>
        UniformToFill,
        /// <summary>
        /// The content is resized to fill the destination when stretched by its parent. It keeps its ratio otherwise.
        /// </summary>
        /// <userdoc>The content is resized to fill the destination when stretched by its parent. It keeps its ratio otherwise.</userdoc>
        FillOnStretch,
        /// <summary>
        /// The content is always resized to fill the destination dimensions. The aspect ratio is not preserved.
        /// </summary>
        /// <userdoc>The content is always resized to fill the destination dimensions. The aspect ratio is not preserved.</userdoc>
        Fill,
    }
}
