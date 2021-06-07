using ICSharpCode.AvalonEdit.CodeCompletion;

namespace Stride.Roslyn.Editor
{
    public static class AvalonEditExtensions
    {
        public static bool IsOpen(this CompletionWindowBase window) => window?.IsVisible == true;
    }
}