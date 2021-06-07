using ICSharpCode.AvalonEdit.Highlighting;

namespace Stride.Roslyn.Editor
{
    public interface IClassificationHighlightColors
    {
        HighlightingColor DefaultBrush { get; }

        HighlightingColor GetBrush(string classificationTypeName);
    }
}