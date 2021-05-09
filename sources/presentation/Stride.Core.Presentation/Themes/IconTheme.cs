// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows.Media;

using Stride.Core.Presentation.Drawing;

namespace Stride.Core.Presentation.Themes
{
    public struct IconTheme
    {
        public IconTheme(string name, Color backgroundColor)
        {
            Name = name;
            BackgroundColor = backgroundColor;
        }

        public string Name { get; }

        public Color BackgroundColor { get; }

        public double BackgroundLuminosity => BackgroundColor.ToHslColor().Luminosity;

    }
}
