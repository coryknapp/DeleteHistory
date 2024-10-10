using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using Task = System.Threading.Tasks.Task;

namespace DeleteHistory
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(DeleteHistoryPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(DeleteHistoryWindow))]
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideOptionPage(typeof(DeleteHistoryOptionsPage), "Delete History", "General", 0, 0, true)]
    [ProvideProfile(typeof(DeleteHistoryOptionsPage), "Delete History", "General", 0, 0, true)]
    public sealed class DeleteHistoryPackage : ToolkitPackage
    {
        public static DeleteHistoryPackage Package { get; set; }

        public ObservableCollection<DeleteHistoryRecordViewModel> Buttons { get; set; }

        /// <summary>
        /// DeleteHistoryPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "a9127e51-cd76-4db2-a3bb-ba99de63fd93";

       protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await DeleteHistoryWindowCommand.InitializeAsync(this);

            this.Buttons = new ObservableCollection<DeleteHistoryRecordViewModel>();

            Package = this;
        }

        public void AddHistory(DeleteHistoryRecordViewModel viewModel)
        {
            Buttons.Add(viewModel);
        }

        private ICommand ButtonCommand(string text)
        {
            return new PasteCommand(text);
        }

        private string ButtonText(string text)
        {
            return text;
        }
    }
}
