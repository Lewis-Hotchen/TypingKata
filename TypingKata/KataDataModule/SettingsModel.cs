using System;
using System.Linq;
using GalaSoft.MvvmLight;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;

namespace KataDataModule {

    /// <summary>
    /// Model for the Settings.
    /// </summary>
    public class SettingsModel : ViewModelBase {
        private readonly ISettingsRepository _settingsRepository;

        private bool _isLearnMode;
        public string DefaultFilePath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData;

        /// <summary>
        /// Instantiate new Settings Model.
        /// </summary>
        /// <param name="settingsRepository"></param>
        public SettingsModel(ISettingsRepository settingsRepository) {
            _settingsRepository = settingsRepository;
            TryLoadSettings();
        }

        /// <summary>
        /// Try and load the settings if they are available.
        /// </summary>
        private void TryLoadSettings() {
            foreach (var settingJsonObject in _settingsRepository.Settings) {
                switch (settingJsonObject.Name) {
                    //As more settings are added, add here to load them.
                    case nameof(IsLearnMode):
                        _isLearnMode = (bool) settingJsonObject.Data;
                        RaisePropertyChanged(nameof(IsLearnMode));
                        break;
                }
            }
        }

        /// <summary>
        /// Toggle Learn mode for the test.
        /// </summary>
        public bool IsLearnMode {
            get => _isLearnMode;
            set {
                UpdateSetting(value, nameof(IsLearnMode));
                Set(ref _isLearnMode, value);
            }
        }

        /// <summary>
        /// Update the setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="property"></param>
        public void UpdateSetting<T>(T value, string property) {
            if (_settingsRepository.Settings.FirstOrDefault(x => x.Name == property) == null) {
                _settingsRepository.Settings.Add(new SettingJsonObject(value, property));
            }

            _settingsRepository.Settings.First(x => x.Name == property).Data = value;
            _settingsRepository.WriteOutSettings();
        }
    }
}