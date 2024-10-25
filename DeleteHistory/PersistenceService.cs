using System;
using System.Collections.Generic;
using Community.VisualStudio.Toolkit;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Settings;
using System.Text.Json;
using System.Linq;

namespace DeleteHistory
{
    public class PersistenceService
    {
        private WritableSettingsStore Store;

        const string CollectionPath = "CollectionPathSolutionSettings";
        const string HistoryKey = "History";

        public PersistenceService()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);

            this.Store = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            // Create collection if it doesn't exist
            if (!this.Store.CollectionExists(CollectionPath))
            {
                this.Store.CreateCollection(CollectionPath);
            }
        }

        public void SaveHistory(IList<DeleteHistoryEntry> history)
        {
            string jsonString = JsonSerializer.Serialize(history.ToList());
            this.Store.SetString(CollectionPath, HistoryKey, jsonString);
        }

        public IList<DeleteHistoryEntry> LoadHistory()
        {
            if (!this.Store.PropertyExists(CollectionPath, HistoryKey))
            {
                return new List<DeleteHistoryEntry>();
            }

            string jsonString = Store.GetString(CollectionPath, HistoryKey);
            return JsonSerializer.Deserialize<List<DeleteHistoryEntry>>(jsonString);
        }
    }
}
