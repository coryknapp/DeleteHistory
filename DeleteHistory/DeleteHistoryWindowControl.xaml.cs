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
        public ObservableCollection<DeleteHistoryRecordViewModel> Buttons
        {
            get
            {
                return DeleteHistoryPackage.Package.Buttons;
            }
        }

        public DeleteHistoryWindowControl()
        {
            this.InitializeComponent();
            DataContext = this;
        }
    }
}