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
        private ITinyMessengerHub _messengerHub { get; }

        private IDataSerializer _serializer;
        private bool _isLearnMode;

        public SettingsModel(IDataSerializer serializer, ITinyMessengerHub messengerHub) {
            _messengerHub = messengerHub;
            _serializer = serializer;
        }

        public bool IsLearnMode {
            get => _isLearnMode;
            set {
                _messengerHub.Publish(new ToggleSettingUpdated(this, value));
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData + @"\" + Resources.SettingsData;
                _serializer.SerializeObject(new SettingJsonObject(value, nameof(IsLearnMode)), path);
                _messengerHub.Publish(new JsonUpdatedMessage(this));
                Set(ref _isLearnMode, value);
            }
        }
    }
}