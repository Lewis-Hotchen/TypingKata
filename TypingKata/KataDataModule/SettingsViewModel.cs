using System;
using System.Collections.Generic;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using KataIocModule;

namespace KataDataModule {
    public class SettingsViewModel : ViewModelBase {
        private readonly IDataSerializer _serializer;
        private readonly IJSonLoader _loader;

        private readonly SettingsModel _model;

        public SettingsViewModel(IDataSerializer serializer, IJSonLoader loader, ITinyMessengerHub messengerHub, ISettingsRepository settingsRepository) {
            _serializer = serializer;
            _loader = loader;
            _model = new SettingsModel(serializer, messengerHub, loader, settingsRepository);
            ResetDataCommand = new RelayCommand(ResetData);
            _model.PropertyChanged += ModelOnPropertyChanged;
        }

        public RelayCommand ResetDataCommand { get; }

        public bool IsLearnModeOn {
            get => _model.IsLearnMode;
            set  {
                _model.IsLearnMode = value;
            } 
        }

        private void ResetData() {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData + @"\" + Resources.TypingResults;
            _serializer.SerializeObject(new List<WPMJsonObject>(), path);
            _loader.RefreshJsonFiles();
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e) {

            RaisePropertyChanged(nameof(IsLearnModeOn));
        }

    }
}
