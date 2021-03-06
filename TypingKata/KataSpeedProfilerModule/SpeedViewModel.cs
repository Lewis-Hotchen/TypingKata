﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
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
        private const string TextPath = "KataSpeedProfilerModule.Resources.words.txt";
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");

        private ITypingProfiler _profiler;
        private bool _textFocus;
        private double _testTime;
        private string _currentChar;
        private string _words;
        private bool _isRunning;
        private string _word;

        /// <summary>
        /// Gets the Start Test Command.
        /// </summary>
        public RelayCommand StartTestCommand { get; }

        /// <summary>
        /// Gets the Document.
        /// </summary>
        public FlowDocument Document { get; set; }

        /// <summary>
        /// Gets the typing profiler.
        /// </summary>
        public ITypingProfiler TypingProfiler {
            get => _profiler;
            private set => Set(ref _profiler, value);
        }

        /// <summary>
        /// Gets the text focus.
        /// </summary>
        public bool TextFocus {
            get => _textFocus;
            set => Set(ref _textFocus, value);
        }

        /// <summary>
        /// Gets the Words.
        /// </summary>
        public string Words {
            get => _words;
            set => Set(ref _words, value);
        }

        /// <summary>
        /// Gets the current word.
        /// </summary>
        public string CurrentWord {
            get => _word;
            set => Set(ref _word, value);
        }

        /// <summary>
        /// Gets the current test time.
        /// </summary>
        public double TestTime {
            get => _testTime;
            set {
                Set(ref _testTime, value);
                StartTestCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets the Current Character.
        /// </summary>
        public string CurrentChar {
            get => _currentChar;
            set => Set(ref _currentChar, value);
        }

        /// <summary>
        /// Instantiate new SpeedViewModel.
        /// </summary>
        /// <param name="typingProfilerFactory"></param>
        public SpeedViewModel(ITypingProfilerFactory typingProfilerFactory) {
            _typingProfilerFactory = typingProfilerFactory;
            StartTestCommand = new RelayCommand(StartTest, StartTestCanExecute);
            Document = new FlowDocument { FontSize = 40, FontFamily = new FontFamily("Segoe UI"), PagePadding = new Thickness(0)};
            _isRunning = false;
        }

        /// <summary>
        /// Predicate to determine if you can start test.
        /// </summary>
        /// <returns></returns>
        private bool StartTestCanExecute() {
            return _testTime >= 60 && _isRunning == false;
        }

        /// <summary>
        /// Start the test.
        /// </summary>
        private void StartTest() {
            Log.Debug($"Test started with {TestTime} time");
            CreateProfiler();
            TypingProfiler?.Start();
            TextFocus = true;
            _words = "";
            
            if (TypingProfiler?.GeneratedWords != null)
                foreach (var word in TypingProfiler?.GeneratedWords) {
                    Words += string.Join("", word.Chars.Select(x => x.CurrentCharacter));
                }

            _isRunning = true;
            StartTestCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Create the profiler.
        /// </summary>
        private void CreateProfiler() {
            TypingProfiler = _typingProfilerFactory.CreateTypingProfiler(
                BootStrapper.Resolve<ICursor>(),
                BootStrapper.Resolve<IWordStack>(),
                BootStrapper.Resolve<ITypingTimer>(new Parameter[] {new NamedParameter("time", TestTime)}),
                BootStrapper.Resolve<IMarkovChainGenerator>(new Parameter[] {new NamedParameter("path", TextPath)}));
            TypingProfiler.KeyComplete += ProfilerOnKeyComplete;
            TypingProfiler.Cursor.CharacterChangedEvent += CursorOnCharacterChangedEvent;
            TypingProfiler.NextWordEvent += TypingProfilerOnNextWordEvent;
            TypingProfiler.TestCompleteEvent += TypingProfilerOnTestCompleteEvent;
            TypingProfiler.BackspaceCompleteEvent += TypingProfilerOnBackspaceCompleteEvent;
        }

        /// <summary>
        /// Handle the backspace.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypingProfilerOnBackspaceCompleteEvent(object sender, BackspaceCompleteEvent e) {
            BackspaceDocumentText();
            if (e.IsError == CharacterStatus.Correct) {
                Words = Words.Insert(0, e.AddedChar.ToString());
            }
        }

        private void BackspaceDocumentText() {
            var para = (Paragraph) Document.Blocks.FirstOrDefault(p => p.GetType() == typeof(Paragraph));
            var inline = (Run) para?.Inlines.LastInline;

            if (inline != null) {
                if (inline.Text.Length > 0) {
                    inline.Text = inline.Text.Remove(inline.Text.Length - 1);
                    return;
                }
            }

            if (inline?.Text.Length == 0) {
                para.Inlines.Remove(inline);
            }

            RaisePropertyChanged(nameof(Document));
        }

        /// <summary>
        /// Handle end of test event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypingProfilerOnTestCompleteEvent(object sender, TestCompleteEventArgs e) {

            Words = " ";
            RaisePropertyChanged(nameof(Words));

            Document.Dispatcher.Invoke(() =>
            {
                Document.Blocks.Clear();
                RaisePropertyChanged(nameof(Document));
            });
            
            _isRunning = false;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => StartTestCommand.RaiseCanExecuteChanged()));
        }

        /// <summary>
        /// Handle next word event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypingProfilerOnNextWordEvent(object sender, WordChangedEventArgs e) {
            CurrentChar = e.NewWord[0].CurrentCharacter;
            Words = Words.Remove(0, 1);
            RaisePropertyChanged(nameof(Words));
            var tr = new TextRange(Document.ContentEnd, Document.ContentEnd) { Text = " " };
            RaisePropertyChanged(nameof(Document));
        }
        
        /// <summary>
        /// Handle character changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CursorOnCharacterChangedEvent(object sender, CharacterChangedEventArgs e) {
            CurrentChar = e.NewValue.ToString() == " " ? "space" : e.NewValue.ToString();
        }

        /// <summary>
        /// Handle the key complete after typing profiler has processed it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProfilerOnKeyComplete(object sender, KeyInputEventHandlerArgs e) {
            CurrentWord = TypingProfiler.Cursor.CurrentWord.ToString();
            var isKeyCorrect = e.IsCorrect;
            Log.Debug($"Key pressed : {e.InputKey}");
            var status = isKeyCorrect ? CharacterStatus.Correct : CharacterStatus.Incorrect;

            if (e.InputKey == ' ' || e.InputKey == '\b') return;

            var tr = new TextRange(Document.ContentEnd, Document.ContentEnd)
                {Text = new string(new[] {e.InputKey})};

            switch (status) {
                case CharacterStatus.Correct:
                    tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.AliceBlue);
                    if (Words != "") {
                        Words = Words.Remove(0, 1);
                    }
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