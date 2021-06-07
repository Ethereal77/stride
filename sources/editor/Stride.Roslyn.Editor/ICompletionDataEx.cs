using ICSharpCode.AvalonEdit.CodeCompletion;

namespace Stride.Roslyn.Editor
{
    public interface ICompletionDataEx : ICompletionData
    {
        bool IsSelected { get; }

        string SortText { get; }
    }

    public interface IOverloadProviderEx : IOverloadProvider
    {
        void Refresh();
    }
}