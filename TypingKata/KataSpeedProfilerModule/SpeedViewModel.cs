using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Autofac;
using Autofac.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataIocModule;
using KataSpeedProfilerModule.Interfaces;
using log4net;

namespace KataSpeedProfilerModule {
    public class SpeedViewModel : ViewModelBase {
        private readonly ITypingProfilerFactory _typingProfilerFactory;
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        private readonly SpeedModel _model;
        private ITypingProfiler _profiler;
        public RelayCommand StartTestCommand { get; }
        private readonly IValueConverter _keyToCharConverter;

        public SpeedViewModel(ITypingProfilerFactory typingProfilerFactory, IValueConverter keyToCharConverter) {
            _typingProfilerFactory = typingProfilerFactory;
            _keyToCharConverter = keyToCharConverter;
            _model = new SpeedModel();
            Log.Debug($"model is {_model}");
            StartTestCommand = new RelayCommand(StartTest);
        }

        public string Words { get; set; }
        public double TestTime { get; set; }
        public Key KeyPressed { get; set; }
        public bool IsKeyCorrect { get; set; }

        /// <summary>
        /// Start the test.
        /// </summary>
        private void StartTest() {
            _profiler = _typingProfilerFactory.CreateTypingProfiler(
                BootStrapper.Resolve<ICursor>(),
                BootStrapper.Resolve<IWordStack>(),
                BootStrapper.Resolve<IWordQueue>(),
                BootStrapper.Resolve<ITypingTimer>(new Parameter[] { new NamedParameter("time", TestTime) }));
            _profiler.KeyComplete += ProfilerOnKeyComplete;
            _profiler.Start(5, 50);
            var words = _profiler.Queue.GetWordQueueAsArray().Select(x => x.ToString());
            Words = string.Join("_", words);
            RaisePropertyChanged(nameof(Words));
            var c = _profiler.Cursor.CurrentWord[0];
            // ReSharper disable once PossibleNullReferenceException
            var key = (Key) _keyToCharConverter.ConvertBack(c, typeof(char), null, CultureInfo.InvariantCulture);
            _profiler.CharacterInput(key);
        }

        private void ProfilerOnKeyComplete(object sender, KeyInputEventHandlerArgs e) {
            KeyPressed = e.InputKey;
            IsKeyCorrect = e.IsCorrect;
            RaisePropertyChanged(nameof(KeyPressed));
            RaisePropertyChanged(nameof(IsKeyCorrect));
        }
    }
}