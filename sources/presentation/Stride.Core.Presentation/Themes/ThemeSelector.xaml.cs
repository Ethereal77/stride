// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Stride.Core.Presentation.Themes
{
    public partial class ThemeSelector : ResourceDictionary
    {
        public ThemeSelector()
        {
            InitializeComponent();
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image img && img.Source is DrawingImage drawingImage)
            {
                img.Source = new DrawingImage
                {
                    Drawing = ImageThemingUtilities.TransformDrawing(drawingImage.Drawing, ThemeController.CurrentTheme.GetThemeBase().GetIconTheme())
                };
            }
        }
    }
}
