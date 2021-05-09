// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Rendering.Images;
using Stride.Rendering.Compositing;

namespace Stride.Rendering
{
    /// <summary>
    ///   Defines the interface for a image effect that can be used by a <see cref="SceneRendererBase"/>.
    /// </summary>
    /// <remarks>
    ///   An <see cref="IImageEffectRenderer"/> expects an input texture on slot 0, possibly a depth texture
    ///   on slot 1 and a single output.
    /// </remarks>
    public interface IImageEffectRenderer : IImageEffect { }
}
