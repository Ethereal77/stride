// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Stride.Rendering.Images
{
    /// <summary>
    ///   Defines the common interface for a screen-space anti-aliasing effect.
    /// </summary>
    public interface IScreenSpaceAntiAliasingEffect : IImageEffect
    {
        bool NeedRangeDecompress { get; }
        bool RequiresDepthBuffer { get; }
        bool RequiresVelocityBuffer { get; }
    }
}
