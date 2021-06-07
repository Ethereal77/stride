using System;
using Microsoft.CodeAnalysis.Text;
using Stride.Roslyn.EditorServices.Diagnostics;

namespace Stride.Roslyn.EditorServices
{
    public class DocumentCreationArgs
    {
        public DocumentCreationArgs(SourceTextContainer sourceTextContainer, string workingDirectory, Action<DiagnosticsUpdatedArgs>? onDiagnosticsUpdated = null, Action<SourceText>? onTextUpdated = null, string? name = null)
        {
            SourceTextContainer = sourceTextContainer;
            WorkingDirectory = workingDirectory;
            OnDiagnosticsUpdated = onDiagnosticsUpdated;
            OnTextUpdated = onTextUpdated;
            Name = name;
        }

        public SourceTextContainer SourceTextContainer { get; }
        public string WorkingDirectory { get; }
        public Action<DiagnosticsUpdatedArgs>? OnDiagnosticsUpdated { get; }
        public Action<SourceText>? OnTextUpdated { get; }
        public string? Name { get; }
    }
}
