using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly SettingsModel _model;

        public SettingsViewModel(IDataSerializer serializer, IJSonLoader loader, ITinyMessengerHub messengerHub) {
            _serializer = serializer;
            _loader = loader;
            _messengerHub = messengerHub;
            _model = new SettingsModel(serializer, messengerHub, loader);
            ResetDataCommand = new RelayCommand(ResetData);
            _model.PropertyChanged += ModelOnPropertyChanged;
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e) {
            RaisePropertyChanged(nameof(IsLearnModeOn));
        }

        public bool IsLearnModeOn {
            get => _model.IsLearnMode;
            set  {
                _model.IsLearnMode = value;
            } 
        }

        private void ResetData() {
            var results = _loader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults);
            results?.Clear();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData + @"\" + Resources.TypingResults;
            _serializer.SerializeObject(results, path);
            _loader.RefreshJsonFiles();
        }

        public RelayCommand ResetDataCommand { get; }

    }
}
