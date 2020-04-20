// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2016, Eli Arbel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host.Mef;

using RoslynPad.Editor;
using RoslynPad.Roslyn;
using RoslynPad.Roslyn.Diagnostics;

namespace Stride.Assets.Presentation.AssetEditors.ScriptEditor
{
    /// <summary>
    /// Manages services needed by Roslyn.
    /// </summary>
    public class RoslynHost : IRoslynHost
    {
        private readonly RoslynWorkspace workspace;
        private readonly CompositionHost compositionContext;
        private readonly MefHostServices hostServices;

        private readonly ConcurrentDictionary<DocumentId, Action<DiagnosticsUpdatedArgs>> diagnosticsUpdatedNotifiers = new ConcurrentDictionary<DocumentId, Action<DiagnosticsUpdatedArgs>>();

        public RoslynHost()
        {
            compositionContext = CreateCompositionContext();

            // Create MEF host services
            hostServices = MefHostServices.Create(compositionContext);

            // Create default workspace
            workspace = new RoslynWorkspace(this);
            workspace.EnableDiagnostics(DiagnosticOptions.Semantic | DiagnosticOptions.Syntax);

            GetService<IDiagnosticService>().DiagnosticsUpdated += OnDiagnosticsUpdated;

            ParseOptions = CreateDefaultParseOptions();
        }

        private static CompositionHost CreateCompositionContext()
        {
            var assemblies = new[]
            {
                Assembly.Load("Microsoft.CodeAnalysis.Workspaces"),
                Assembly.Load("Microsoft.CodeAnalysis.CSharp.Workspaces"),
                Assembly.Load("Microsoft.CodeAnalysis.Features"),
                Assembly.Load("Microsoft.CodeAnalysis.CSharp.Features"),
                Assembly.Load("Microsoft.CodeAnalysis.Workspaces.MSBuild"),
                typeof(IRoslynHost).Assembly, // RoslynPad.Roslyn
                typeof(SymbolDisplayPartExtensions).Assembly, // RoslynPad.Roslyn.Windows
                typeof(AvalonEditTextContainer).Assembly, // RoslynPad.Editor.Windows
            };

            var partTypes = assemblies
                .SelectMany(x => x.DefinedTypes)
                .Select(x => x.AsType());

            return new ContainerConfiguration()
                .WithParts(partTypes)
                .CreateContainer();
        }

        internal static readonly ImmutableArray<string> PreprocessorSymbols =
            ImmutableArray.CreateRange(new[] { "TRACE", "DEBUG" });

        protected virtual ParseOptions CreateDefaultParseOptions()
        {
            return new CSharpParseOptions(kind: SourceCodeKind.Regular,
                preprocessorSymbols: PreprocessorSymbols, languageVersion: LanguageVersion.Latest);
        }

        /// <summary>
        /// The roslyn workspace.
        /// </summary>
        public RoslynWorkspace Workspace => workspace;

        /// <summary>
        /// The roslyn services.
        /// </summary>
        public MefHostServices HostServices => hostServices;

        public ParseOptions ParseOptions { get; }

        /// <summary>
        /// Gets a specific service.
        /// </summary>
        /// <typeparam name="TService">The type of service to get.</typeparam>
        /// <returns>The service if found, [null] otherwise.</returns>
        public TService GetService<TService>()
        {
            return compositionContext.GetExport<TService>();
        }

        private void OnDiagnosticsUpdated(object sender, DiagnosticsUpdatedArgs diagnosticsUpdatedArgs)
        {
            var documentId = diagnosticsUpdatedArgs?.DocumentId;
            if (documentId == null) return;

            Action<DiagnosticsUpdatedArgs> notifier;
            if (diagnosticsUpdatedNotifiers.TryGetValue(documentId, out notifier))
            {
                notifier(diagnosticsUpdatedArgs);
            }
        }

        public DocumentId AddDocument(DocumentCreationArgs args)
        {
            throw new NotImplementedException();
        }

        public Document GetDocument(DocumentId documentId)
        {
            return workspace.CurrentSolution.GetDocument(documentId);
        }

        public void CloseDocument(DocumentId documentId)
        {
            workspace.CloseDocument(documentId);
        }

        public MetadataReference CreateMetadataReference(string location)
        {
            throw new NotImplementedException();
        }
    }
}
