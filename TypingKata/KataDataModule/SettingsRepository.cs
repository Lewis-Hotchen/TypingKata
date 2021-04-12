using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;

namespace KataDataModule {

    /// <summary>
    /// Class to store and update settings.
    /// </summary>
    public class SettingsRepository : ISettingsRepository {
        private readonly string _settingsPath;
        private readonly IJSonLoader _loader;
        private readonly IDataSerializer _serializer;
        private ObservableCollection<SettingJsonObject> _observableSettings;

        /// <summary>
        /// Collection of the Settings.
        /// </summary>
        public IList<SettingJsonObject> Settings => _observableSettings;

        /// <summary>
        /// Event that fires when settings are updated.
        /// </summary>
        public event EventHandler SettingsUpdatedEvent;

        /// <summary>
        /// Get a Setting by string index.
        /// </summary>
        /// <param name="index">The name of the setting.</param>
        /// <returns></returns>
        public SettingJsonObject this[string index] => _observableSettings.FirstOrDefault(x => x.Name == index);

         /// <summary>
        /// Instantiate new Settings Repository.
        /// </summary>
        /// <param name="settingsPath"></param>
        /// <param name="serializer"></param>
        /// <param name="loader"></param>
        public SettingsRepository(string settingsPath, IDataSerializer serializer, IJSonLoader loader) {
            _settingsPath = settingsPath;
            _loader = loader;
            _serializer = serializer;
            LoadSettings();
        }

        /// <summary>
        /// Load the settings.
        /// </summary>
        private void LoadSettings() {
            var settings = _loader.LoadTypeFromJson<List<SettingJsonObject>>(Resources.SettingsData);

            if (settings == null) {
                _observableSettings = new ObservableCollection<SettingJsonObject>();
                WriteOutSettings();
            } else {
                _observableSettings = new ObservableCollection<SettingJsonObject>(settings);
            }

            _observableSettings.CollectionChanged += ObservableSettingsOnCollectionChanged;
        }

        /// <summary>
        /// Write out settings on collection changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ObservableSettingsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            WriteOutSettings();
        }

        /// <summary>
        /// Write out the settings.
        /// </summary>
        public void WriteOutSettings() {
            _serializer.SerializeObject(Settings, _settingsPath + @"\" + Resources.SettingsData);
            _loader.RefreshJsonFiles();
            SettingsUpdatedEvent?.Invoke(this, System.EventArgs.Empty);
        }
    }
}