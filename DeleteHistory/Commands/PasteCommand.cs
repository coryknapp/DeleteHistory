using Community.VisualStudio.Toolkit;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;


namespace DeleteHistory
{
    internal class PasteCommand : ICommand
    {
        private readonly string text;

        public PasteCommand(string text)
        {
            this.text = text;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            DocumentView docView = VS.Documents.GetActiveDocumentViewAsync().Result;
            if (docView?.TextView == null) return;
            SnapshotPoint position = docView.TextView.Caret.Position.BufferPosition;
            docView.TextBuffer?.Insert(position, text);
        }
    }
}