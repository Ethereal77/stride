using Microsoft.CodeAnalysis;

namespace Stride.Roslyn.EditorServices
{
    public interface IRoslynHost
    {
        ParseOptions ParseOptions { get; }

        TService GetService<TService>();

        DocumentId AddDocument(DocumentCreationArgs args);

        Document? GetDocument(DocumentId documentId);

        void CloseDocument(DocumentId documentId);

        MetadataReference CreateMetadataReference(string location);
    }
}
