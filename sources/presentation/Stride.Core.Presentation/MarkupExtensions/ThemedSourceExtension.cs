// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows.Markup;
using System.Windows.Media;

using Stride.Core.Annotations;
using Stride.Core.Presentation.Themes;

namespace Stride.Core.Presentation.MarkupExtensions
{
    using static Stride.Core.Presentation.Themes.IconThemeSelector;

    [MarkupExtensionReturnType(typeof(ImageSource))]
    public class ThemedSourceExtension : MarkupExtension
    {
        public ThemedSourceExtension() { }

        public ThemedSourceExtension(ImageSource source, KnownThemes theme)
        {
            Source = source;
            Theme = theme.GetIconTheme();
        }

        [ConstructorArgument("source")]
        private ImageSource Source { get; }

        [ConstructorArgument("theme")]
        private IconTheme Theme { get; }

        [NotNull]
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source is DrawingImage drawingImage)
            {
                return new DrawingImage
                {
                    Drawing = ImageThemingUtilities.TransformDrawing(drawingImage.Drawing, Theme)
                };
            }
            else
            {
                return Source;
            }
        }
    }
}
