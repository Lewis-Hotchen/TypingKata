using System.Collections.Generic;
using GalaSoft.MvvmLight;
using KataDataModule.EventArgs;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using KataIocModule;

namespace KataDataModule {
    public class AnalyticsModel : ViewModelBase {

        private readonly IJSonLoader _jSonLoader;

        public AnalyticsModel(IJSonLoader jSonLoader, ITinyMessengerHub messengerHub) {
            _jSonLoader = jSonLoader;
            WpmResults = _jSonLoader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults);
            if (WpmResults == null) {
                WpmResults = new List<WPMJsonObject>();
            }
            messengerHub.Subscribe<JsonUpdatedMessage>(DeliveryAction);
        }

        private void DeliveryAction(JsonUpdatedMessage obj) {
            WpmResults = _jSonLoader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults);
            RaisePropertyChanged(nameof(WpmResults));
        }

        public List<WPMJsonObject> WpmResults { get; private set; }
    }
}