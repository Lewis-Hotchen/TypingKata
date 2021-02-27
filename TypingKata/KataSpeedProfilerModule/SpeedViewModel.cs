using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Autofac;
using Autofac.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataIocModule;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;
using log4net;
using TinyMessenger;

namespace KataSpeedProfilerModule {
    public class SpeedViewModel : ViewModelBase {

        private readonly ITypingProfilerFactory _typingProfilerFactory;
        private readonly ITinyMessengerHub _messengerHub;
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        private readonly SpeedModel _model;
        private ITypingProfiler _profiler;
        private bool _textFocus;
        private double _testTime;
        private string _currentChar;
        private ObservableCollection<IWord> _words;
        private const string TextPath = "KataSpeedProfilerModule.Resources.words.txt";

        public RelayCommand StartTestCommand { get; }

        public FlowDocument Document { get; set; }

        public ITypingProfiler TypingProfiler {
            get => _profiler;
            private set => Set(ref _profiler, value);
        }

        public bool TextFocus {
            get => _textFocus;
            set => Set(ref _textFocus, value);
        }

        public string Words => _words == null ? null : string.Join("", _words.Select(x => x.ToString()));

        public double TestTime {
            get => _testTime;
            set {
                Set(ref _testTime, value);
                StartTestCommand.RaiseCanExecuteChanged();
            }
        }

        public string CurrentChar {
            get => _currentChar;
            set => Set(ref _currentChar, value);
        }

        public SpeedViewModel(ITypingProfilerFactory typingProfilerFactory, ITinyMessengerHub messengerHub) {
            _typingProfilerFactory = typingProfilerFactory;
            _messengerHub = messengerHub;
            _model = new SpeedModel();
            Log.Debug($"model is {_model}");
            StartTestCommand = new RelayCommand(StartTest, StartTestCanExecute);
            Document = new FlowDocument {FontSize = 40, FontFamily = new FontFamily("Segoe UI"), PagePadding = new Thickness(0)};
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
            TypingProfiler?.Start(50);
            TextFocus = true;
            _words = new ObservableCollection<IWord>();
            if (TypingProfiler?.GeneratedWords != null)
                foreach (var word in TypingProfiler?.GeneratedWords) {
                    _words.Add(new GeneratedWord(string.Join("", word.Chars.Select(x => x.CurrentCharacter))));
                }

            RaisePropertyChanged(nameof(Words));
        }

        private void CreateProfiler() {
            TypingProfiler = _typingProfilerFactory.CreateTypingProfiler(
                BootStrapper.Resolve<ICursor>(),
                BootStrapper.Resolve<IWordStack>(),
                BootStrapper.Resolve<IWordQueue>(),
                BootStrapper.Resolve<ITypingTimer>(new Parameter[] {new NamedParameter("time", TestTime)}),
                BootStrapper.Resolve<IMarkovChainGenerator>(new Parameter[] {new NamedParameter("path", TextPath)}));
            TypingProfiler.KeyComplete += ProfilerOnKeyComplete;
            TypingProfiler.Cursor.CharacterChangedEvent += CursorOnCharacterChangedEvent;
            TypingProfiler.NextWordEvent += TypingProfilerOnNextWordEvent;
            TypingProfiler.TestCompleteEvent += TypingProfilerOnTestCompleteEvent;
            TypingProfiler.BackspaceCompleteEvent += TypingProfilerOnBackspaceCompleteEvent;
        }

        private void TypingProfilerOnBackspaceCompleteEvent(object sender, BackspaceComleteEvent e) {
            var para = (Paragraph)Document.Blocks.FirstOrDefault(p => p.GetType() == typeof(Paragraph));
            var inline = (Run) para?.Inlines.LastInline;

            if (inline != null) {
                if (inline.Text.Length > 0) {
                    inline.Text = inline.Text.Remove(inline.Text.Length - 1);
                }
            }

            if (inline?.Text.Length == 0) {
                para.Inlines.Remove(inline);
            }

            RaisePropertyChanged(nameof(Document));
        }

        private void TypingProfilerOnTestCompleteEvent(object sender, TestCompleteEventArgs e) {
            var messageBoxText = $"Test Complete! Wpm was {e.Wpm}";
            var caption = "Speed Profiler";
            var button = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;

            MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void TypingProfilerOnNextWordEvent(object sender, WordChangedEventArgs e) {
            CurrentChar = e.NewWord[0].CurrentCharacter;
            //_words.RemoveAt(0);
            RaisePropertyChanged(nameof(Words));
            var tr = new TextRange(Document.ContentEnd, Document.ContentEnd) { Text = " " };
            RaisePropertyChanged(nameof(Document));
        }


        private void CursorOnCharacterChangedEvent(object sender, CharacterChangedEventArgs e) {
            CurrentChar = e.NewValue.ToString() == " " ? "space" : e.NewValue.ToString();
        }

        /// <summary>
        /// Handle the key complete after typing profiler has processed it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProfilerOnKeyComplete(object sender, KeyInputEventHandlerArgs e) {
            var isKeyCorrect = e.IsCorrect;
            Log.Debug($"Key pressed : {e.InputKey}");

            var status = isKeyCorrect ? CharacterStatus.Correct : CharacterStatus.Incorrect;

            if (e.InputKey != ' ' && e.InputKey != '\b') {
                RaisePropertyChanged(nameof(Words));
                var tr = new TextRange(Document.ContentEnd, Document.ContentEnd)
                    {Text = new string(new[] {e.InputKey})};

                switch (status) {
                    case CharacterStatus.Correct:
                        tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.AliceBlue);
                        break;
                    case CharacterStatus.Incorrect:
                        tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
                        break;
                    case CharacterStatus.Unmodified:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                RaisePropertyChanged(nameof(Document));
            }
        }
    }
}