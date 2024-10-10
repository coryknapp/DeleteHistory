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
        [DisplayName("Truncate lines")]
        [Description("Only display this many lines in the history window")]
        [DefaultValue(12)]
        public int TruncateLines { get; set; } = 12;
    }
}
