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
        private SpeedModel _model;

        public RelayCommand NextWordCommand { get; }

        public SpeedViewModel() {
            _model = new SpeedModel(BootStrapper.Resolve<ICursor>(), 
                BootStrapper.Resolve<ITypingTimer>(new Parameter[]{new NamedParameter("time", 60d)}));
            Log.Debug($"model is {_model}");
            NextWordCommand = new RelayCommand(NextWord);
            _model.TypingTimer.TimeComplete += TypingTimerOnTimeComplete;
            _model.TypingTimer.StartTimer();
        }

        private void TypingTimerOnTimeComplete(object sender, EventArgs e) {
            ;
        }

        private void NextWord() {
            Cursor.NextWord(0, new Word("Hello"));
            RaisePropertyChanged(nameof(CurrentWord));
        }


        public string CurrentWord => Cursor.CurrentWord?.ToString();
        public ICursor Cursor => _model.Cursor;
    }
}
