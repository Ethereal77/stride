using System.Threading.Tasks;

namespace Stride.Roslyn.Editor
{
    public interface ICodeEditorCompletionProvider
    {
        Task<CompletionResult> GetCompletionData(int position, char? triggerChar, bool useSignatureHelp);
    }
}