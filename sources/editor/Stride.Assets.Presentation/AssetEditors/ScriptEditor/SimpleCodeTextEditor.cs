// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org)
// Copyright (c) 2018-2021 Stride and its contributors (https://stride3d.net)
// Copyright (c) 2011-2018 Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// See the LICENSE.md file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.CodeAnalysis;

using Stride.Roslyn.Editor;
using Stride.Roslyn.EditorServices.BraceMatching;
using Stride.Roslyn.EditorServices.Diagnostics;
using Stride.Roslyn.EditorServices.QuickInfo;

using Stride.Core.Presentation.Themes;

namespace Stride.Assets.Presentation.AssetEditors.ScriptEditor
{
    /// <summary>
    ///   A <see cref="CodeTextEditor"/> with <em>IntelliSense</em> connected to Stride's <see cref="RoslynWorkspace"/>.
    /// </summary>
    public class SimpleCodeTextEditor : CodeTextEditor
    {
        private RoslynWorkspace workspace;
        private DocumentId documentId;
        private AvalonEditTextContainer sourceTextContainer;

        private RoslynHighlightingColorizer syntaxHighlightingColorizer;
        private TextMarkerService textMarkerService;
        private ContextActionsRenderer contextActionsRenderer;
        private RoslynContextActionProvider contextActionProvider;
        private IQuickInfoProvider quickInfoProvider;

        private BraceMatcherHighlightRenderer braceMatcherHighlighter;
        private IBraceMatchingService braceMatchingService;
        private CancellationTokenSource braceMatchingCts;

        public static readonly StyledProperty<ImageSource> ContextActionsIconProperty = CommonProperty.Register<SimpleCodeTextEditor, ImageSource>
            (
                nameof(ContextActionsIcon),
                onChanged: OnContextActionsIconChanged
            );

        private static void OnContextActionsIconChanged(SimpleCodeTextEditor editor, CommonPropertyChangedArgs<ImageSource> args)
        {
            if (editor.contextActionsRenderer is not null)
                editor.contextActionsRenderer.IconImage = args.NewValue;
        }

        public ImageSource ContextActionsIcon
        {
            get => this.GetValue(ContextActionsIconProperty);
            set => this.SetValue(ContextActionsIconProperty, value);
        }


        /// <summary>
        ///   Connects the text editor to a Roslyn document and a <see cref="AvalonEditTextContainer"/> to setup syntax highlighting, Intellisense, etc.
        /// </summary>
        public void BindSourceTextContainer(RoslynWorkspace workspace, AvalonEditTextContainer sourceTextContainer, DocumentId documentId)
        {
            this.workspace = workspace;
            this.sourceTextContainer = sourceTextContainer;
            this.documentId = documentId;

            Document = sourceTextContainer.Document;

            // Update Caret position on text update
            sourceTextContainer.Editor = this;

            // Setup text markers (underline diagnostics)
            textMarkerService = new TextMarkerService(this);
            TextArea.TextView.BackgroundRenderers.Add(textMarkerService);
            TextArea.TextView.LineTransformers.Add(textMarkerService);
            TextArea.Caret.PositionChanged += CaretOnPositionChanged;

            // Syntax highlighting
            var classificationHighlightColors = ThemeController.CurrentTheme.GetThemeBase() == IconThemeSelector.ThemeBase.Dark ? new ClassificationHighlightColorsDark() : new ClassificationHighlightColors();
            syntaxHighlightingColorizer = new RoslynHighlightingColorizer(documentId, workspace.Host, classificationHighlightColors);
            TextArea.TextView.LineTransformers.Insert(0, syntaxHighlightingColorizer);

            // Context Actions
            contextActionsRenderer = new ContextActionsRenderer(this, textMarkerService) { IconImage = ContextActionsIcon };
            contextActionProvider = new RoslynContextActionProvider(documentId, workspace.Host);
            contextActionsRenderer.Providers.Add(contextActionProvider);

            // Completion provider
            CompletionProvider = new RoslynCodeEditorCompletionProvider(documentId, workspace.Host);
            // TODO: Warmup() is internal

            // Quick info
            quickInfoProvider = workspace.Host.GetService<IQuickInfoProvider>();

            // Brace matching
            braceMatcherHighlighter = new BraceMatcherHighlightRenderer(TextArea.TextView, classificationHighlightColors);
            braceMatchingService = workspace.Host.GetService<IBraceMatchingService>();
            AsyncToolTipRequest = ProcessAsyncToolTipRequest;
        }

        /// <summary>
        ///   Disconnects the text editor from a Roslyn document.
        /// </summary>
        public void Unbind()
        {
            // Caret/brace matching
            TextArea.Caret.PositionChanged -= CaretOnPositionChanged;

            // Syntax highlighting
            TextArea.TextView.LineTransformers.Remove(syntaxHighlightingColorizer);

            // Text markers
            TextArea.TextView.BackgroundRenderers.Remove(textMarkerService);
            TextArea.TextView.LineTransformers.Remove(textMarkerService);
            textMarkerService = null;

            // Tooltips
            AsyncToolTipRequest = null;

            // Context Actions
            contextActionsRenderer.Providers.Remove(contextActionProvider);
            contextActionsRenderer = null;
            contextActionProvider = null;

            // Completion provider
            CompletionProvider = null;

            // No need to update caret position anymore
            sourceTextContainer.Editor = null;

            workspace = null;
            sourceTextContainer = null;
            documentId = null;
        }

        public void ProcessDiagnostics(DiagnosticsUpdatedArgs args)
        {
            if (Dispatcher.CheckAccess())
            {
                ProcessDiagnosticsOnUiThread(args);
                return;
            }

            Dispatcher.InvokeAsync(() => ProcessDiagnosticsOnUiThread(args));
        }

        private void ProcessDiagnosticsOnUiThread(DiagnosticsUpdatedArgs args)
        {
            textMarkerService.RemoveAll(marker => Equals(args.Id, marker.Tag));

            if (args.Kind != DiagnosticsUpdatedKind.DiagnosticsCreated)
                return;

            foreach (var diagnosticData in args.Diagnostics)
            {
                if (diagnosticData.Severity == DiagnosticSeverity.Hidden || diagnosticData.IsSuppressed)
                    continue;

                var text = diagnosticData.GetTextSpan() ?? default;
                var marker = textMarkerService.TryCreate(text.Start, text.Length);
                if (marker != null)
                {
                    marker.Tag = args.Id;
                    marker.MarkerColor = GetDiagnosticsColor(diagnosticData);
                    marker.ToolTip = diagnosticData.Message;
                }
            }
        }

        private async void CaretOnPositionChanged(object sender, EventArgs eventArgs)
        {
            braceMatchingCts?.Cancel();

            if (braceMatchingService is null)
                return;

            var cts = new CancellationTokenSource();
            var token = cts.Token;
            braceMatchingCts = cts;

            var document = workspace.Host.GetDocument(documentId);
            try
            {
                var text = await document.GetTextAsync(token).ConfigureAwait(false);
                var caretOffset = CaretOffset;
                if (caretOffset <= text.Length)
                {
                    var (leftOfPosition, rightOfPosition) = await braceMatchingService.GetAllMatchingBracesAsync(document, caretOffset, token).ConfigureAwait(true);
                    braceMatcherHighlighter.SetHighlight(leftOfPosition, rightOfPosition);
                }
            }
            catch (OperationCanceledException) { }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
            {
                switch (e.Key)
                {
                    case Key.OemCloseBrackets:
                        TryJumpToBrace();
                        break;
                }
            }
        }

        private void TryJumpToBrace()
        {
            if (braceMatcherHighlighter is null)
                return;

            var caret = CaretOffset;

            if (TryJumpToPosition(braceMatcherHighlighter.LeftOfPosition, caret) ||
                TryJumpToPosition(braceMatcherHighlighter.RightOfPosition, caret))
            {
                ScrollToLine(TextArea.Caret.Line);
            }
        }

        private bool TryJumpToPosition(BraceMatchingResult? position, int caret)
        {
            if (position != null)
            {
                if (position.Value.LeftSpan.Contains(caret))
                {
                    CaretOffset = position.Value.RightSpan.End;
                    return true;
                }

                if (position.Value.RightSpan.Contains(caret) || position.Value.RightSpan.End == caret)
                {
                    CaretOffset = position.Value.LeftSpan.Start;
                    return true;
                }
            }

            return false;
        }

        private static Color GetDiagnosticsColor(DiagnosticData diagnosticData) =>
            diagnosticData.Severity switch
            {
                DiagnosticSeverity.Info => Colors.LimeGreen,
                DiagnosticSeverity.Warning => Colors.DodgerBlue,
                DiagnosticSeverity.Error => Colors.Red,

                _ => throw new ArgumentOutOfRangeException()
            };

        private async Task ProcessAsyncToolTipRequest(ToolTipRequestEventArgs arg)
        {
            // TODO: Consider invoking this with a delay, then showing the tool-tip without one
            var document = workspace.GetDocument(documentId);
            var info = await quickInfoProvider.GetItemAsync(document, arg.Position, CancellationToken.None).ConfigureAwait(true);
            if (info != null)
            {
                arg.SetToolTip(info.Create());
            }
        }
    }
}
