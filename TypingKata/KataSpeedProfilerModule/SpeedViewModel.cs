using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataSpeedProfilerModule.Properties;
using log4net;

namespace KataSpeedProfilerModule {
    public class SpeedViewModel : ViewModelBase {
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        private SpeedModel _model { get; }

        public RelayCommand NextWordCommand { get; }
        public RelayCommand StartTest { get; }

        public SpeedViewModel() {
            _model = new SpeedModel();
            Log.Debug($"model is {_model}");
            NextWordCommand = new RelayCommand(NextWord);
            StartTest = new RelayCommand(StartTestAction);
            //_model.TypingTimer.TimeComplete += TypingTimerOnTimeComplete;
            //_model.TypingTimer.StartTimer();
        }

        public string Words { get; set; }

        private void StartTestAction() {
            //setup test code here
            //_model.TypingTimer.StartTimer();
        }

        private void TypingTimerOnTimeComplete(object sender, EventArgs e) {
            //logic here to stop test
            
        }

        private void NextWord() {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "KataSpeedProfilerModule.Resources.words.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException())) {
                var result = reader.ReadToEnd();
                var words = MarkovChainTextGenerator.Markov(result.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                ), 3, 50);

                Words = words.Replace(" ", "_");
                RaisePropertyChanged(nameof(Words));
            }
        }


       // public string CurrentWord => Cursor.CurrentWord?.ToString();
       // public ICursor Cursor => _model.Cursor;
    }
}
