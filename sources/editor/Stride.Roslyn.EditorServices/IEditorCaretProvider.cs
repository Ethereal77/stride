namespace Stride.Roslyn.EditorServices
{
    public interface IEditorCaretProvider
    {
        int CaretPosition { get; }

        bool TryMoveCaret(int position);
    }
}
