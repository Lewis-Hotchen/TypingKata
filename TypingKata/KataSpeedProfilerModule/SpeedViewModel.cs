using System;
using System.Windows.Input;
using Autofac;
using Autofac.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataIocModule;
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

        private void StartTestAction() {
            //setup test code here
            //_model.TypingTimer.StartTimer();
        }

        private void TypingTimerOnTimeComplete(object sender, EventArgs e) {
            //logic here to stop test
            
        }

        private void NextWord() {
            //Cursor.NextWord(0, new Word("Hello"));
            //RaisePropertyChanged(nameof(CurrentWord));
        }


       // public string CurrentWord => Cursor.CurrentWord?.ToString();
       // public ICursor Cursor => _model.Cursor;
    }
}
