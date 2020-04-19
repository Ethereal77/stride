// Copyright (c) 2018-2020 Xenko and its contributors (https://xenko.com)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Xenko.Core.Annotations;
using Xenko.Core.Mathematics;

namespace Xenko.Core.Presentation.Extensions
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
