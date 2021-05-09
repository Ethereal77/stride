// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core;

namespace Stride.Rendering.Images.Dither
{
    [DataContract("Dither")]
    public class Dither : ColorTransform
    {
        public Dither() : base("Dither")
        {
        }

        public override void UpdateParameters(ColorTransformContext context)
        {
            base.UpdateParameters(context);

            Parameters.Set(DitherKeys.Time, (float)(context.RenderContext.Time.Total.TotalSeconds));
        }
    }
}
