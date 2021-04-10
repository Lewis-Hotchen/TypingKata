using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataDataModule.EventArgs;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using KataIocModule;

namespace KataDataModule {
    public class SettingsViewModel : ViewModelBase {
        private readonly IDataSerializer _serializer;
        private readonly IJSonLoader _loader;
        private readonly ITinyMessengerHub _messengerHub;
        private SettingsModel _model;
        private bool _dataResetVisible;

        public SettingsViewModel(IDataSerializer serializer, IJSonLoader loader, ITinyMessengerHub messengerHub) {
            _serializer = serializer;
            _loader = loader;
            _messengerHub = messengerHub;
            _model = new SettingsModel(serializer);
            ResetDataCommand = new RelayCommand(ResetData);
        }

        public bool DataResetVisible {
            get => _dataResetVisible;
            set => Set(ref _dataResetVisible, value);
        }

        private void ResetData() {
            var results = _loader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults);
            results?.Clear();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData + @"\" + Resources.TypingResults;
            _serializer.SerializeObject(results, path);
            _messengerHub.Publish(new JsonUpdatedMessage(this));
        }

        public RelayCommand ResetDataCommand { get; }

    }
}
