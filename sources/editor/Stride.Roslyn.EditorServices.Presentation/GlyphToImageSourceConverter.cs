using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Stride.Roslyn.EditorServices.Completion;

namespace Stride.Roslyn.EditorServices.Presentation
{
    [ValueConversion(typeof(Glyph), typeof(ImageSource))]
    public class GlyphToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Glyph?)?.ToImageSource() ?? Binding.DoNothing;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
