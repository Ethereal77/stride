// Copyright (c) 2018-2020 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Copyright (c) 2016, Eli Arbel
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host.Mef;

using RoslynPad.Roslyn;
using RoslynPad.Roslyn.Diagnostics;
using RoslynPad.Editor;

namespace Stride.Assets.Presentation.AssetEditors.ScriptEditor
{
    /// <summary>
    ///   Manages the services needed by Roslyn.
    /// </summary>
    public class RoslynHost : IRoslynHost
    {
        private readonly RoslynWorkspace workspace;
        private readonly CompositionHost compositionContext;
        private readonly MefHostServices hostServices;

        private readonly ConcurrentDictionary<DocumentId, Action<DiagnosticsUpdatedArgs>> diagnosticsUpdatedNotifiers = new();

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
                typeof(IRoslynHost).Assembly,                               // RoslynPad.Roslyn
                typeof(SymbolDisplayPartExtensions).Assembly,               // RoslynPad.Roslyn.Windows
                typeof(AvalonEditTextContainer).Assembly                    // RoslynPad.Editor.Windows
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
            return new CSharpParseOptions
                (
                    kind: SourceCodeKind.Regular,
                    preprocessorSymbols: PreprocessorSymbols,
                    languageVersion: LanguageVersion.Latest
                );
        }


        /// <summary>
        ///   Gets the Roslyn workspace.
        /// </summary>
        public RoslynWorkspace Workspace => workspace;

        /// <summary>
        ///   Gets the registered Roslyn services.
        /// </summary>
        public MefHostServices HostServices => hostServices;

        /// <summary>
        ///   Gets the configured parsed options for parsing C# with Roslyn.
        /// </summary>
        public ParseOptions ParseOptions { get; }


        /// <summary>
        ///   Gets a specific Roslyn service.
        /// </summary>
        /// <typeparam name="TService">The type of service to get.</typeparam>
        /// <returns>The service if found; <c>null</c> otherwise.</returns>
        public TService GetService<TService>() => compositionContext.GetExport<TService>();

        private void OnDiagnosticsUpdated(object sender, DiagnosticsUpdatedArgs diagnosticsUpdatedArgs)
        {
            var documentId = diagnosticsUpdatedArgs?.DocumentId;
            if (documentId is null)
                return;

            if (diagnosticsUpdatedNotifiers.TryGetValue(documentId, out Action<DiagnosticsUpdatedArgs> notifier))
            {
                notifier(diagnosticsUpdatedArgs);
            }
        }

        public DocumentId AddDocument(DocumentCreationArgs args) => throw new NotImplementedException();

        public Document GetDocument(DocumentId documentId) => workspace.CurrentSolution.GetDocument(documentId);

        public void CloseDocument(DocumentId documentId) => workspace.CloseDocument(documentId);

        public MetadataReference CreateMetadataReference(string location) => throw new NotImplementedException();
    }
}
