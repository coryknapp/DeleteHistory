using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.PlatformUI;
using System.Linq;
using System;
using Microsoft.VisualStudio.Text.Operations;
using EnvDTE;
using Community.VisualStudio.Toolkit;
using System.Windows;

namespace DeleteHistory
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal class DeleteTextChangeListener : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService EditorAdaptersFactoryService { get; set; }

        [Import]
        internal ITextDocumentFactoryService TextDocumentFactoryService { get; set; }

        [Import]
        private ITextUndoHistoryRegistry UndoHistoryRegistry { get; set; }
        private ITextUndoHistory _undoHistory;

        private IVsTextView TextViewAdapter;

        private bool UndoFlag;

        private ITextDocument GetDocument()
        {
            IWpfTextView wpfTextView = EditorAdaptersFactoryService.GetWpfTextView(TextViewAdapter);

            TextViewAdapter.GetBuffer(out var buffer);
            if (TextDocumentFactoryService.TryGetTextDocument(wpfTextView.TextBuffer, out ITextDocument document))
            {
                return document;
            }
            else
            {
                throw new Exception("could not get document");
            }
        }


        public DeleteTextChangeListener()
        {
        }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            if(TextViewAdapter != null)
            {
                if (TextViewAdapter == textViewAdapter)
                {
                    throw new Exception();
                }

            }

            TextViewAdapter = textViewAdapter;

            ITextBuffer buffer = this.GetDocument().TextBuffer;

            buffer.Changed += OnTextBufferChanged;

            // Get the undo history for the text buffer
            UndoHistoryRegistry.TryGetHistory(buffer, out _undoHistory);
        }

        private void OnTextBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            // Respond to text changes
            foreach (var change in e.Changes)
            {
                if(this.IsChangeEligible(change))
                {
                    string filePath;

                    if (TextDocumentFactoryService.TryGetTextDocument(e.After.TextBuffer, out ITextDocument textDocument))
                    {
                        filePath = textDocument.FilePath;
                    }
                    else
                    {
                        filePath = "Unknown file";
                    }

                    var viewModel = new DeleteHistoryEntry()
                    {
                        FileName = filePath,
                        DeleteTime = DateTime.Now,
                        DeletedText = change.OldText,
                    };

                    DeleteHistoryPackage.Package.AddHistory(viewModel);
                }
            }

            UndoFlag = false;
        }

        private bool IsChangeEligible(ITextChange change) =>
            !string.IsNullOrWhiteSpace(change.OldText)
            && change.OldText.Length > DeleteHistoryOptions.Instance.MinimumBlockLength
            && change.OldText.Count(c => c == '\n') > DeleteHistoryOptions.Instance.MinimumLineCount
            && !IsUndoAction(change);

        // Still trying to figure out the best way to handle this.
        // this at least covers the case where you undo a paste action
        private bool IsUndoAction(ITextChange change) =>
            change.OldText == Clipboard.GetText();
    }
}
