using Community.VisualStudio.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeleteHistory
{

    [ComVisible(true)]
    public class DeleteHistoryOptionsPage : BaseOptionPage<DeleteHistoryOptions> { }

    public class DeleteHistoryOptions : BaseOptionModel<DeleteHistoryOptions>
    {
        [Category("Threshold")]
        [DisplayName("Minimum Block Length")]
        [Description("Only retain deletes of this length and larger")]
        [DefaultValue(12)]
        public int MinimumBlockLength { get; set; } = 12;

        [Category("Threshold")]
        [DisplayName("Minimum line count")]
        [Description("Only retain deletes with this many lines")]
        [DefaultValue(0)]
        public int MinimumLineCount { get; set; } = 0;

        [Category("Display")]
        [DisplayName("Show source file name")]
        [Description("Display source file in the history window")]
        [DefaultValue(true)]
        public bool ShowSourceFileName { get; set; } = true;

        [Category("Display")]
        [DisplayName("Show Delete Time")]
        [Description("Display the delete time in the history window")]
        [DefaultValue(true)]
        public bool ShowDeleteTime { get; set; } = true;

        [Category("Display")]
        [DisplayName("Truncate lines")]
        [Description("Limit number of lines shown in the history window")]
        [DefaultValue(false)]
        public bool TruncateLines{ get; set; } = false;

        [Category("Display")]
        [DisplayName("Truncate lines")]
        [Description("Only display this many lines in the history window (if Truncate lines is checked.)")]
        [DefaultValue(12)]
        public int TruncateLinesCount { get; set; } = 12;

        [Category("Retention")]
        [DisplayName("Maximum History Count")]
        [Description("Maximum history count (zero for unlimited)")]
        [DefaultValue(256)]
        public int MaximumHistoryCount { get; set; } = 256;
    }
}
