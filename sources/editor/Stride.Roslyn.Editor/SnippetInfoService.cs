using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Stride.Roslyn.EditorServices.Snippets;

namespace Stride.Roslyn.Editor
{
    [Export(typeof(ISnippetInfoService)), Shared]
    internal sealed class SnippetInfoService : ISnippetInfoService
    {
        public SnippetManager SnippetManager { get; } = new SnippetManager();

        public IEnumerable<SnippetInfo> GetSnippets()
        {
            return SnippetManager.Snippets.Select(x => new SnippetInfo(x.Name, x.Name, x.Description));
        }
    }
}