using System.Windows.Media;
using Stride.Roslyn.EditorServices.Completion;
using Stride.Roslyn.Resources;

namespace Stride.Roslyn.EditorServices.Presentation
{
    public static class GlyphExtensions
    {
        private static readonly GlyphService _service = new GlyphService();

        public static ImageSource? ToImageSource(this Glyph glyph) => _service.GetGlyphImage(glyph);

        private class GlyphService
        {
            private readonly Glyphs _glyphs;

            public GlyphService()
            {
                _glyphs = new Glyphs();
            }

            public ImageSource? GetGlyphImage(Glyph glyph) => _glyphs[glyph] as ImageSource;
        }
    }
}
