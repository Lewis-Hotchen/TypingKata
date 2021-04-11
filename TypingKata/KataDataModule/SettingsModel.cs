using System;
using System.Collections.Generic;
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
        private bool _isLearnMode;
        private readonly string _settingsPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData + @"\" + Resources.SettingsData;

        public SettingsModel(IDataSerializer serializer, ITinyMessengerHub messengerHub, IJSonLoader loader) {
            _messengerHub = messengerHub;
            _serializer = serializer;
            _loader = loader;
            TryLoadSettings();
        }

        private void TryLoadSettings() {
            var settings = _loader.LoadTypeFromJson<List<SettingJsonObject>>(Resources.SettingsData);

            if (settings == null) {
                settings = new List<SettingJsonObject>();
                _serializer.SerializeObject(settings, _settingsPath);
            }
            else {
                foreach (var settingJsonObject in settings) {
                    //As more settings are added, add here to load them.
                    if (settingJsonObject.Name == nameof(IsLearnMode)) {
                        IsLearnMode = (bool) settingJsonObject.Data;
                    }
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

        public void UpdateSetting<T>(T value, string property) {
            var settings = _loader.LoadTypeFromJson<List<SettingJsonObject>>(Resources.SettingsData);

            var setting = settings.Find(x => x.Name == property);

            if (setting != null) {
                setting.Data = value;
                _serializer.SerializeObject(settings, _settingsPath);
            }
            else {
                settings.Add(new SettingJsonObject(value, property));
                _serializer.SerializeObject(settings, _settingsPath);
            }

            _loader.RefreshJsonFiles();
        }
    }
}