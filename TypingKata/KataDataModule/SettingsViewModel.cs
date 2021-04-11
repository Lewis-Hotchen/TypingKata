using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataDataModule.Interfaces;
using KataIocModule;

namespace KataDataModule {
    public class SettingsViewModel : ViewModelBase {

        private readonly SettingsModel _model;

        public SettingsViewModel(IDataSerializer serializer, IJSonLoader loader, ITinyMessengerHub messengerHub, ISettingsRepository settingsRepository) {
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
            _model.ResetData();
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e) {

            RaisePropertyChanged(nameof(IsLearnModeOn));
        }

    }
}
