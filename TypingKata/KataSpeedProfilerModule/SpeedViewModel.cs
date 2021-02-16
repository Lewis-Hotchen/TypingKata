using System.Linq;
using System.Windows;
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
using Microsoft.Win32;

namespace KataSpeedProfilerModule {
    public class SpeedViewModel : ViewModelBase {

        private readonly ITypingProfilerFactory _typingProfilerFactory;
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        private readonly SpeedModel _model;
        private readonly IValueConverter _keyToCharConverter;
        private ITypingProfiler _profiler;
        private string[] _words;
        private bool _textFocus;
        private double _testTime;
        private char _keyPressed;
        private bool _isKeyCorrect;
        private string _currentChar;
        private const string TextPath = "KataSpeedProfilerModule.Resources.words.txt";

        public RelayCommand StartTestCommand { get; }

        public ITypingProfiler TypingProfiler {
            get => _profiler;
            private set => Set(ref _profiler, value);
        }

        public bool TextFocus {
            get => _textFocus;
            set => Set(ref _textFocus, value);
        }

        public string DisplayWords => string.Join("_", _words);

        public double TestTime {
            get => _testTime;
            set {
                Set(ref _testTime, value);
                StartTestCommand.RaiseCanExecuteChanged();
            } 
        }

        public char KeyPressed {
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
            _words = new string[] { }; 
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
            CreateProfiler();
            TypingProfiler?.Start();
            RefreshWords();
            TextFocus = true;
        }

        private void CreateProfiler() {
            TypingProfiler = _typingProfilerFactory.CreateTypingProfiler(
                BootStrapper.Resolve<ICursor>(),
                BootStrapper.Resolve<IWordStack>(),
                BootStrapper.Resolve<IWordQueue>(),
                BootStrapper.Resolve<ITypingTimer>(new Parameter[] { new NamedParameter("time", TestTime) }),
                BootStrapper.Resolve<IMarkovChainGenerator>(new Parameter[] { new NamedParameter("path", TextPath) }));
            TypingProfiler.KeyComplete += ProfilerOnKeyComplete;
            TypingProfiler.Cursor.CharacterChangedEvent += CursorOnCharacterChangedEvent;
            TypingProfiler.NextWordEvent += TypingProfilerOnNextWordEvent;
            TypingProfiler.TestCompleteEvent += TypingProfilerOnTestCompleteEvent;
        }

        private void TypingProfilerOnTestCompleteEvent(object sender, TestCompleteEventArgs e) {
            RefreshWords();
            var messageBoxText = $"Test Complete! Wpm was {e.Wpm}";
            var caption = "Speed Profiler";
            var button = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;

            MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void TypingProfilerOnNextWordEvent(object sender, CharacterChangedEventArgs e) {
            CurrentChar = e.NewValue.ToString();
            RefreshWords();
        }

        private void CursorOnCharacterChangedEvent(object sender, CharacterChangedEventArgs e) {
            CurrentChar = e.NewValue.ToString() == " " ? "space" : e.NewValue.ToString();
        }

        private void RefreshWords() {
            _words = TypingProfiler.Queue.GetWordQueueAsArray().Select(x => x.ToString()).ToArray();
            RaisePropertyChanged(nameof(DisplayWords));
        }

        private void ProfilerOnKeyComplete(object sender, KeyInputEventHandlerArgs e) {
            KeyPressed = e.InputKey;
            IsKeyCorrect = e.IsCorrect;
            Log.Debug($"Key pressed : {e.InputKey}");
        }
    }
}