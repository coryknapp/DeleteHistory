using System;
using System.Windows.Input;

namespace DeleteHistory
{
    public class DeleteHistoryEntry
    {
        public string FileName { get; set; }
        
        public DateTime DeleteTime { get; set; }

        public string DeletedText { get; set; }    // The text to display on the button
    }
}