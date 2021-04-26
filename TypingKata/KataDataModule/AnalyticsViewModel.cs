using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using KataIocModule;

namespace KataDataModule {
    public class AnalyticsViewModel : ViewModelBase {

        private readonly AnalyticsModel _model;

        /// <summary>
        /// Instantiate new Analytics viewmodel.
        /// </summary>
        /// <param name="loader">The JsonLoader.</param>
        /// <param name="messenger">The TinyMessengerHub.</param>
        /// <param name="resultsRepository"></param>
        public AnalyticsViewModel(ITinyMessengerHub messenger, ITypingResultsRepository resultsRepository) {
            _model = new AnalyticsModel(messenger, resultsRepository);
            _model.PropertyChanged += ModelOnPropertyChanged;
            WpmResults = new ObservableCollection<WPMJsonObject>(_model.WpmResults);
            MostMisspelledWord();
            
        }

        /// <summary>
        /// Event fired on model property changed.
        /// </summary>
        /// <param name="sender">Sender of the event (model).</param>
        /// <param name="e">Event Arguments.</param>
        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e) {
            WpmResults = new ObservableCollection<WPMJsonObject>(_model.WpmResults);
            RaisePropertyChanged(nameof(WpmResults));
            RaisePropertyChanged(nameof(WpmAverage));
            RaisePropertyChanged(nameof(MostMisspelled));
            RaisePropertyChanged(nameof(TotalTimeSpent));
            RaisePropertyChanged(nameof(TotalNumberOfTests));
            RaisePropertyChanged(nameof(AverageErrorRate));
        }

        /// <summary>
        /// List of all test results.
        /// </summary>
        public ObservableCollection<WPMJsonObject> WpmResults { get; private set; }

        /// <summary>
        /// Most misspelled word.
        /// </summary>
        public string MostMisspelled {
            get {
                var (item1, item2) = MostMisspelledWord();
                return item1 + "(" + item2 + ")";
            }
        }

        /// <summary>
        /// Get's the wpm average from results.
        /// </summary>
        public double WpmAverage {
            get {
                if(_model.WpmResults.Any())
                    return Math.Round(_model.WpmResults.Average(x => x.Wpm), 2);
                return 0;
            }
        }

        /// <summary>
        /// Get's the total time spent.
        /// </summary>
        public int TotalTimeSpent => _model.WpmResults.Any() ? Convert.ToInt32(_model.WpmResults.Sum(x => x.Time)) : 0;

        /// <summary>
        /// Gets the total number of tests done.
        /// </summary>
        public int TotalNumberOfTests => _model.WpmResults.Any() ? _model.WpmResults.Count : 0;

        public double AverageErrorRate => Math.Round(AverageErrorRateCalculation(), 2);

        private double AverageErrorRateCalculation() {
            if (!_model.WpmResults.Any()) return 0;

            return _model.WpmResults.Average(x => x.ErrorRate);
        }

        /// <summary>
        /// This gets all of the incorrect words in the results, finds the word that is misspelled the most and returns it.
        /// </summary>
        /// <returns>Returns the most misspelled word.</returns>
        public Tuple<string, int> MostMisspelledWord() {

            if (_model.WpmResults == null || _model.WpmResults.Count == 0) {
                return new Tuple<string, int>("No Data", 0);
            }

            var incorrectWords = _model.WpmResults.Select(x => x.IncorrectWords).SelectMany(x => x);

            if (!incorrectWords.Any()) {
                return new Tuple<string, int>("No Data", 0);
            }

            var res = incorrectWords.Select(x => x.Item2).GroupBy(
                    k => k,
                    StringComparer.InvariantCultureIgnoreCase)
                .Select(v => (v.Key, v.Count())).ToArray();

            var max = res.Max(x => x.Item2);
            var (item1, item2) = res.First(x => x.Item2 == max);

            return new Tuple<string, int>(item1, item2);
        }
    }
}
