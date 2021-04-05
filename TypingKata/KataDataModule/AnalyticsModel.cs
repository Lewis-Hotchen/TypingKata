using System.Collections.Generic;
using KataIocModule;

namespace KataDataModule {
    public class AnalyticsModel {

        private readonly IJSonLoader _jSonLoader;

        public AnalyticsModel(IJSonLoader jSonLoader, ITinyMessengerHub messengerHub) {
            _jSonLoader = jSonLoader;
            WpmResults = _jSonLoader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults);
            messengerHub.Subscribe<JsonUpdatedMessage>(DeliveryAction);
        }

        private void DeliveryAction(JsonUpdatedMessage obj) {
            WpmResults = _jSonLoader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults);
        }

        public List<WPMJsonObject> WpmResults { get; private set; }

    }
}
