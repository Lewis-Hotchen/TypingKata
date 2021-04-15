using System;
using System.Collections.Generic;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using KataIocModule;

namespace KataDataModule {

    /// <summary>
    /// View Model for the settings.
    /// </summary>
    public class SettingsViewModel : ViewModelBase {
        private readonly IDataSerializer _serializer;
        private readonly IJSonLoader _loader;

        private readonly SettingsModel _model;

        /// <summary>
        /// Instantiate new Settings View Model.
        /// </summary>
        /// <param name="serializer">The Data Serializer.</param>
        /// <param name="loader">The Json Loader.</param>
        /// <param name="messengerHub">The TinyMessengerHub.</param>
        /// <param name="settingsRepository">The SettingsRepository.</param>
        public SettingsViewModel(IDataSerializer serializer, IJSonLoader loader, ITinyMessengerHub messengerHub, ISettingsRepository settingsRepository) {
            _serializer = serializer;
            _loader = loader;
            _model = new SettingsModel(serializer, messengerHub, loader, settingsRepository);
            ResetDataCommand = new RelayCommand(ResetData);
            _model.PropertyChanged += ModelOnPropertyChanged;
        }

        /// <summary>
        /// Command to Reset all data.
        /// </summary>
        public RelayCommand ResetDataCommand { get; }

        /// <summary>
        /// Toggle for the learn mode setting.
        /// </summary>
        public bool IsLearnModeOn {
            get => _model.IsLearnMode;
            set  {
                _model.IsLearnMode = value;
            } 
        }

        /// <summary>
        /// Reset all the data.
        /// </summary>
        private void ResetData() {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData + @"\" + Resources.TypingResults;
            _serializer.SerializeObject(new List<WPMJsonObject>(), path);
            _loader.RefreshJsonFiles();
        }

        /// <summary>
        /// Property changed event of the model.
        /// </summary>
        /// <param name="sender">sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e) {

            RaisePropertyChanged(nameof(IsLearnModeOn));
        }

    }
}
