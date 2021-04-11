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

        public AnalyticsViewModel(IJSonLoader loader, ITinyMessengerHub messenger) {
            _model = new AnalyticsModel(loader, messenger);
            _model.PropertyChanged += ModelOnPropertyChanged;
            WpmResults = new ObservableCollection<WPMJsonObject>(_model.WpmResults);
            MostMisspelledWord();
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e) {
            RaisePropertyChanged(nameof(WpmResults));
            RaisePropertyChanged(nameof(WpmAverage));
            RaisePropertyChanged(nameof(MostMisspelled));
        }

        public ObservableCollection<WPMJsonObject> WpmResults { get; }

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
                    return _model.WpmResults.Average(x => x.Wpm);
                return 0;
            }
        }

        /// <summary>
        /// This gets all of the incorrect words in the results, finds the word that is misspelled the most and returns it.
        /// </summary>
        /// <returns></returns>
        public Tuple<string, int> MostMisspelledWord() {

            if (_model.WpmResults == null || _model.WpmResults.Count == 0) {
                return new Tuple<string, int>("No Data", 0);
            }

            var incorrectWords = _model.WpmResults.Select(x => x.IncorrectWords).SelectMany(x => x);
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
