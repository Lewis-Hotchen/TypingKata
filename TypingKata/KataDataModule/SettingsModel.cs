using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using KataDataModule.EventArgs;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using KataIocModule;

namespace KataDataModule {
    public class SettingsModel : ViewModelBase {
        private readonly ITinyMessengerHub _messengerHub;
        private readonly IDataSerializer _serializer;
        private readonly IJSonLoader _loader;
        private readonly ISettingsRepository _settingsRepository;

        private bool _isLearnMode;
        private readonly string _settingsPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData + @"\" + Resources.SettingsData;

        public SettingsModel(IDataSerializer serializer, ITinyMessengerHub messengerHub, IJSonLoader loader, ISettingsRepository settingsRepository) {
            _messengerHub = messengerHub;
            _serializer = serializer;
            _loader = loader;
            _settingsRepository = settingsRepository;
            TryLoadSettings();
        }

        /// <summary>
        /// Try and load the settings if they are available.
        /// </summary>
        private void TryLoadSettings() {
            foreach (var settingJsonObject in _settingsRepository.Settings) {
                    //As more settings are added, add here to load them.
                if (settingJsonObject.Name == nameof(IsLearnMode)) {
                    _isLearnMode = (bool) settingJsonObject.Data;
                    RaisePropertyChanged(nameof(IsLearnMode));
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

        public void ResetData() {
            _settingsRepository.Settings.Clear();
        }
    }
}