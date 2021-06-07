using Microsoft.CodeAnalysis;

namespace Stride.Roslyn.EditorServices
{
    // ReSharper disable once InconsistentNaming
    public static class ISymbolExtensions
    {
        public static Completion.Glyph GetGlyph(this ISymbol symbol)
        {
            return (Completion.Glyph)Microsoft.CodeAnalysis.Shared.Extensions.ISymbolExtensions2.GetGlyph(symbol);
        }
    }
}
