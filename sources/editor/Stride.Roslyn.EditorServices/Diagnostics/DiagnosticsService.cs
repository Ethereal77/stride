using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslyn.Utilities;

namespace Stride.Roslyn.EditorServices.Diagnostics
{
    [Export(typeof(IDiagnosticService)), Shared]
    internal sealed class DiagnosticsService : IDiagnosticService
    {
        private readonly Microsoft.CodeAnalysis.Diagnostics.IDiagnosticService _inner;

        [ImportingConstructor]
        public DiagnosticsService(Microsoft.CodeAnalysis.Diagnostics.IDiagnosticService inner)
        {
            _inner = inner;
            inner.DiagnosticsUpdated += OnDiagnosticsUpdated;
        }

        // ReSharper disable once UnusedParameter.Local
        private void OnDiagnosticsUpdated(object? sender, Microsoft.CodeAnalysis.Diagnostics.DiagnosticsUpdatedArgs e)
        {
            DiagnosticsUpdated?.Invoke(this, new DiagnosticsUpdatedArgs(e));
        }

        public event EventHandler<DiagnosticsUpdatedArgs>? DiagnosticsUpdated;

        public ImmutableArray<DiagnosticData> GetDiagnostics(Workspace workspace, ProjectId projectId, DocumentId documentId, object id,
            bool includeSuppressedDiagnostics, CancellationToken cancellationToken)
        {
            return _inner.GetPushDiagnosticsAsync(workspace, projectId, documentId, id, includeSuppressedDiagnostics, InternalDiagnosticsOptions.NormalDiagnosticMode, cancellationToken)
                .AsTask()
                .WaitAndGetResult_CanCallOnBackground(cancellationToken)
                .Select(x => new DiagnosticData(x))
                .ToImmutableArray();
        }

        //public IEnumerable<UpdatedEventArgs> GetDiagnosticsUpdatedEventArgs(Workspace workspace, ProjectId projectId, DocumentId documentId,
        //    CancellationToken cancellationToken)
        //{
        //    return _inner.GetDiagnosticsUpdatedEventArgs(workspace, projectId, documentId, cancellationToken)
        //        .Select(x => new UpdatedEventArgs(x));
        //}
    }
}
