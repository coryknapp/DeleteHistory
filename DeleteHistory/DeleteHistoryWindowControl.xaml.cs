using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace DeleteHistory
{
    /// <summary>
    /// Interaction logic for DeleteHistoryWindowControl.
    /// </summary>
    public partial class DeleteHistoryWindowControl : UserControl
    {
        public ObservableLinkedList<DeleteHistoryEntry> Buttons
        {
            get
            {
                return DeleteHistoryPackage.Package.Entries;
            }
        }

        public DeleteHistoryWindowControl()
        {
            this.InitializeComponent();
            this.Buttons.CollectionChanged += this.CollectionChanged;
            DataContext = this;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ScrollViewer.ScrollToBottom();
        }
    }
}