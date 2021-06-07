using Microsoft.CodeAnalysis.Completion;
using Stride.Roslyn.EditorServices.CodeActions;

namespace Stride.Roslyn.EditorServices.Completion
{
    public static class CompletionItemExtensions
    {
        public static Glyph GetGlyph(this CompletionItem completionItem)
        {
            return CodeActionExtensions.GetGlyph(completionItem.Tags);
        }

        public static CompletionDescription GetDescription(this CompletionItem completionItem)
        {
            return CommonCompletionItem.GetDescription(completionItem);
        }
    }
}