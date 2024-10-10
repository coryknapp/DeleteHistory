using Microsoft.VisualStudio.Shell;
using System;
using System.IO.Packaging;
using System.Runtime.InteropServices;

namespace DeleteHistory
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("fa6aa47d-1bfd-4b8d-8af2-4fe72e149608")]
    public class DeleteHistoryWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteHistoryWindow"/> class.
        /// </summary>
        public DeleteHistoryWindow() : base(null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var package = (DeleteHistoryPackage)ServiceProvider.GlobalProvider.GetService(typeof(DeleteHistoryPackage));

            this.Caption = "Delete History";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new DeleteHistoryWindowControl();
        }
    }
}
