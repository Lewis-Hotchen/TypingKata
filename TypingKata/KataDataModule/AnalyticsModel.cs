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
        private FileSystemWatcher watcher;
        public AnalyticsModel(IJSonLoader jSonLoader, ITinyMessengerHub messengerHub) {

            watcher = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                            @"\" + Resources.TypingKataData) {
                    NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size
            };

            watcher.Changed += WatcherOnChanged;
            _jSonLoader = jSonLoader;
            WpmResults = _jSonLoader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults) ?? new List<WPMJsonObject>();
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e) {
            _jSonLoader.RefreshJsonFiles();
            WpmResults = _jSonLoader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults) ?? new List<WPMJsonObject>();
        }

        public List<WPMJsonObject> WpmResults { get; private set; }
    }
}