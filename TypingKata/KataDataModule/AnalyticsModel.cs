using System;
using System.Collections.Generic;
using System.IO;
using GalaSoft.MvvmLight;
using KataDataModule.EventArgs;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using KataIocModule;

namespace KataDataModule {
    public class AnalyticsModel : ViewModelBase {

        private readonly IJSonLoader _jSonLoader;
        private readonly ITinyMessengerHub _messengerHub;
        private List<WPMJsonObject> _wpmResults;

        public AnalyticsModel(IJSonLoader jSonLoader, ITinyMessengerHub messengerHub, IDataSerializer dataSerializer) {
            _jSonLoader = jSonLoader;
            _messengerHub = messengerHub;
            WpmResults = _jSonLoader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults) ?? new List<WPMJsonObject>();

            if (WpmResults.Count == 0) {
                dataSerializer.SerializeObject(WpmResults, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                           @"\" + Resources.TypingKataData + @"\" + Resources.TypingResults);
            }

            _messengerHub.Subscribe<JsonUpdatedMessage>(JsonUpdatedAction);
        }

        private void JsonUpdatedAction(JsonUpdatedMessage obj) {

            WpmResults = _jSonLoader.LoadTypeFromJsonFile<List<WPMJsonObject>>(Resources.TypingResults) ?? new List<WPMJsonObject>();
        }

        public List<WPMJsonObject> WpmResults {
            get => _wpmResults;
            private set => Set(ref _wpmResults, value);
        }
    }
}