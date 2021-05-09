// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using Stride.Core.Annotations;
using Stride.Core.Mathematics;

namespace Stride.Core.Presentation.Extensions
{
    using SystemColor = System.Windows.Media.Color;

    public static class SystemColorExtensions
    {
        public static SystemColor ToSystemColor(this ColorHSV color)
        {
            return ToSystemColor(color.ToColor());
        }

        public static SystemColor ToSystemColor(this Color color)
        {
            return SystemColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static SystemColor ToSystemColor(this Color4 color4)
        {
            var color = (Color)color4;
            return SystemColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static SystemColor ToSystemColor(this Color3 color3)
        {
            var color = (Color)color3;
            return SystemColor.FromArgb(255, color.R, color.G, color.B);
        }
    }
}
