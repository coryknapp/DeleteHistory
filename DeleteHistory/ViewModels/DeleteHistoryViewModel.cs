using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DeleteHistory
{
    public class DeleteHistoryViewModel : INotifyPropertyChanged
    {
        private string _filterString;
        private ICollectionView _filteredItems;

        public ObservableLinkedList<DeleteHistoryEntry> Entries { get; set; }

        public ICollectionView FilteredItems
        {
            get { return _filteredItems; }
            private set
            {
                _filteredItems = value;
                OnPropertyChanged(nameof(FilteredItems));
            }
        }

        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                OnPropertyChanged(nameof(FilterString));
                FilteredItems.Refresh();
            }
        }

        public DeleteHistoryViewModel()
        {

            FilteredItems = CollectionViewSource.GetDefaultView(DeleteHistoryPackage.Package.Entries);
            FilteredItems.Filter = FilterItems;
        }

        private bool FilterItems(object item)
        {
            if (item is DeleteHistoryEntry entry)
            {
                return string.IsNullOrEmpty(FilterString) || entry.DeletedText.Contains(FilterString);
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
