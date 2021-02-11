using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Autofac;
using Autofac.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataIocModule;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;
using log4net;

namespace KataSpeedProfilerModule {
    public class SpeedViewModel : ViewModelBase {

        private readonly ITypingProfilerFactory _typingProfilerFactory;
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        private readonly SpeedModel _model;
        private readonly IValueConverter _keyToCharConverter;
        private ITypingProfiler _profiler;
        private string _words;
        private bool _textFocus;
        private double _testTime;
        private Key _keyPressed;
        private bool _isKeyCorrect;
        private string _currentChar;

        public RelayCommand StartTestCommand { get; }

        public ITypingProfiler TypingProfiler {
            get => _profiler;
            private set => Set(ref _profiler, value);
        }

        public bool TextFocus {
            get => _textFocus;
            set => Set(ref _textFocus, value);
        }

        public string Words {
            get => _words;
            set => Set(ref _words, value);
        }

        public double TestTime {
            get => _testTime;
            set {
                Set(ref _testTime, value);
                StartTestCommand.RaiseCanExecuteChanged();
            } 
        }

        public Key KeyPressed {
            get => _keyPressed;
            set => Set(ref _keyPressed, value);
        }

        public bool IsKeyCorrect {
            get => _isKeyCorrect;
            set => Set(ref _isKeyCorrect, value);
        }

        public string CurrentChar {
            get => _currentChar;
            set => Set(ref _currentChar, value);
        }

        public SpeedViewModel(ITypingProfilerFactory typingProfilerFactory, IValueConverter keyToCharConverter) {
            _typingProfilerFactory = typingProfilerFactory;
            _keyToCharConverter = keyToCharConverter;
            _model = new SpeedModel();
            Log.Debug($"model is {_model}");
            StartTestCommand = new RelayCommand(StartTest, StartTestCanExecute);
        }

        private bool StartTestCanExecute() {
            return _testTime >= 60;
        }

        /// <summary>
        /// Start the test.
        /// </summary>
        private void StartTest() {
            Log.Debug($"Test started with {TestTime} time");
            TypingProfiler = _typingProfilerFactory.CreateTypingProfiler(
                BootStrapper.Resolve<ICursor>(),
                BootStrapper.Resolve<IWordStack>(),
                BootStrapper.Resolve<IWordQueue>(),
                BootStrapper.Resolve<ITypingTimer>(new Parameter[] { new NamedParameter("time", TestTime) }));
            TypingProfiler.KeyComplete += ProfilerOnKeyComplete;
            TypingProfiler.Cursor.CharacterChangedEvent += CursorOnCharacterChangedEvent;
            TypingProfiler.NextWordEvent += TypingProfilerOnNextWordEvent;
            TypingProfiler.Start(5, 50);
            var words = TypingProfiler.Queue.GetWordQueueAsArray().Select(x => x.ToString());
            Words = string.Join("_", words);
            TextFocus = true;
        }

        private void TypingProfilerOnNextWordEvent(object sender, CharacterChangedEventArgs e) {
            CurrentChar = e.NewValue.ToString();
        }

        private void CursorOnCharacterChangedEvent(object sender, CharacterChangedEventArgs e) {
            CurrentChar = e.NewValue.ToString() == " " ? "space" : e.NewValue.ToString();
        }

        private void ProfilerOnKeyComplete(object sender, KeyInputEventHandlerArgs e) {
            KeyPressed = e.InputKey;
            IsKeyCorrect = e.IsCorrect;
            Log.Debug($"Key pressed : {e.InputKey}");
        }
    }
}