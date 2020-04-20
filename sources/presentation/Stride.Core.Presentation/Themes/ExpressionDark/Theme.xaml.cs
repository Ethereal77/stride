// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Stride.Core.Presentation.Themes.ExpressionDark
{
    public partial class ExpressionDarkTheme : ResourceDictionary
    {
        public ExpressionDarkTheme()
        {
            InitializeComponent();
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image img && img.Source is DrawingImage drawingImage)
            {
                img.Source = new DrawingImage
                {
                    Drawing = ImageThemingUtilities.TransformDrawing(drawingImage.Drawing, IconThemeSelector.KnownThemes.Dark.GetIconTheme())
                };
            }
        }
    }
}
