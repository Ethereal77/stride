using System;
using Microsoft.CodeAnalysis;

namespace Stride.Roslyn.EditorServices.Diagnostics
{
    public class UpdatedEventArgs : EventArgs
    {
        public object Id { get; }

        public Workspace Workspace { get; }

        public ProjectId? ProjectId { get; }

        public DocumentId? DocumentId { get; }

        internal UpdatedEventArgs(Microsoft.CodeAnalysis.Common.UpdatedEventArgs inner)
        {
            Id = inner.Id;
            Workspace = inner.Workspace;
            ProjectId = inner.ProjectId;
            DocumentId = inner.DocumentId;
        }
    }
}
