using System;
using System.Collections.Generic;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using KataIocModule;
using Ookii.Dialogs.Wpf;

namespace KataDataModule {

    /// <summary>
    /// View Model for the settings.
    /// </summary>
    public class SettingsViewModel : ViewModelBase {
        private readonly ITypingResultsRepository _typingResultsRepository;

        private readonly SettingsModel _model;

        /// <summary>
        /// Instantiate new Settings View Model.
        /// </summary>
        /// <param name="messengerHub">The TinyMessengerHub.</param>
        /// <param name="settingsRepository">The SettingsRepository.</param>
        /// <param name="typingResultsRepository">The typing results repository.</param>
        public SettingsViewModel(ITinyMessengerHub messengerHub,
            ISettingsRepository settingsRepository,
            ITypingResultsRepository typingResultsRepository) {
            _typingResultsRepository = typingResultsRepository;
            _model = new SettingsModel(settingsRepository);
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
            set => _model.IsLearnMode = value;
        }

        /// <summary>
        /// Reset all the data.
        /// </summary>
        private void ResetData() {
            _typingResultsRepository.ResetResults();
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
