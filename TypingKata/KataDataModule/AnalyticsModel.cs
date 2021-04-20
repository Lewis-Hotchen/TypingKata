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

        private readonly ITinyMessengerHub _messengerHub;
        private readonly ITypingResultsRepository _resultsRepository;
        private List<WPMJsonObject> _wpmResults;

        /// <summary>
        /// The wpm results.
        /// </summary>
        public List<WPMJsonObject> WpmResults {
            get => _wpmResults;
            set => Set(ref _wpmResults, value);
        } 

        /// <summary>
        /// Initialize new AnalyticsModel.
        /// </summary>
        /// <param name="messengerHub">The tiny messenger hub.</param>
        /// <param name="resultsRepository">The result repository.</param>
        public AnalyticsModel(ITinyMessengerHub messengerHub, ITypingResultsRepository resultsRepository) {
            _messengerHub = messengerHub;
            _resultsRepository = resultsRepository;
            resultsRepository.ResultsChangedEvent += ResultsRepositoryOnResultsChangedEvent;
            _wpmResults = new List<WPMJsonObject>(resultsRepository.Results);
        }

        /// <summary>
        /// Event on the results repository change.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ResultsRepositoryOnResultsChangedEvent(object sender, System.EventArgs e) {
            _wpmResults = new List<WPMJsonObject>(_resultsRepository.Results);
            RaisePropertyChanged(nameof(WpmResults));
        }
    }
}