using System;
using System.Windows.Input;

namespace DeleteHistory
{
    public class DeleteHistoryEntry
    {
        public string FileName { get; set; }
        
        public DateTime DeleteTime { get; set; }

        public string DeletedText { get; set; }    // The text to display on the button
        
        public ICommand ButtonCommand { get; set; }  // The command that will be executed on click
    }
}