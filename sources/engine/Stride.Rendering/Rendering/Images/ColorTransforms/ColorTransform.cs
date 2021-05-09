// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

namespace Stride.Rendering.Images
{
    /// <summary>
    ///   Represents the base class for a color transform to be used in a <see cref="ColorTransformGroup"/>.
    /// </summary>
    public abstract class ColorTransform : ColorTransformBase
    {
        protected ColorTransform(string colorTransformShader)
            : base(colorTransformShader)
        { }
    }
}
