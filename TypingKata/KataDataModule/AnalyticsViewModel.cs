using System;
using System.Collections.ObjectModel;
using System.Linq;
using KataIocModule;

namespace KataDataModule {
    public class AnalyticsViewModel {

        private readonly AnalyticsModel _model;

        public AnalyticsViewModel(IJSonLoader loader, ITinyMessengerHub messenger) {
            _model = new AnalyticsModel(loader, messenger);
            WpmResults = new ObservableCollection<WPMJsonObject>(_model.WpmResults);
            MostMisspelledWord();
        }

        public ObservableCollection<WPMJsonObject> WpmResults { get; private set; }

        public string MostMisspelled {
            get {
                var (item1, item2) = MostMisspelledWord();
                return item1 + "(" + item2 + ")";
            }
        }

        public double WpmAverage {
            get { return _model.WpmResults.Average(x => x.Wpm); }
        }

        /// <summary>
        /// This gets all of the incorrect words in the results, finds the word that is misspelled the most and returns it.
        /// </summary>
        /// <returns></returns>
        public Tuple<string, int> MostMisspelledWord() {

            if (_model?.WpmResults == null) {
                return null;
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
