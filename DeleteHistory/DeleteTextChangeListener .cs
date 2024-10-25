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

        public DeleteTextChangeListener()
        {
        }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            System.Diagnostics.Debug.WriteLine($"VsTextViewCreated");

            IWpfTextView wpfTextView = EditorAdaptersFactoryService.GetWpfTextView(textViewAdapter);

            // Obtain the text view from the adapter
            textViewAdapter.GetBuffer(out var buffer);

            // Get the ITextDocument for the view
            if (TextDocumentFactoryService.TryGetTextDocument(wpfTextView.TextBuffer, out ITextDocument document))
            {
                // Hook up the text changed event
                document.TextBuffer.Changed += OnTextBufferChanged;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"could not get document");
            }
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
                        filePath = "Unknown";
                    }

                    var viewModel = new DeleteHistoryEntry()
                    {
                        FileName = filePath,
                        DeleteTime = DateTime.Now,
                        DeletedText = change.OldText,
                        ButtonCommand = new PasteCommand(change.OldText),
                    };

                    DeleteHistoryPackage.Package.AddHistory(viewModel);
                }
            }

            
        }

        private bool IsChangeEligible(ITextChange change) =>
            !string.IsNullOrWhiteSpace(change.OldText)
            && change.OldText.Length > DeleteHistoryOptions.Instance.MinimumBlockLength
            && change.OldText.Count(c => c == '\n') > DeleteHistoryOptions.Instance.MinimumLineCount;
    }
}
