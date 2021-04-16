using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;
using KataIocModule;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;
using KataSpeedProfilerModule.Properties;
using log4net;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// View model for the Typing Test.
    /// </summary>
    public class SpeedViewModel : ViewModelBase {

        private readonly IDataSerializer _dataSerializer;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IJSonLoader _jsonLoader;
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        private bool _textFocus;
        private double _testTime;
        private string _currentChar;
        private string _words;
        private bool _isRunning;
        private string _word;
        private string _removedWords;
        private readonly SpeedModel _model;
        private bool _learnMode;
        private Brush _currentFingerColor;
        private string _currentFinger;

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
        public ITypingProfiler TypingProfiler => _model.TypingProfiler;

        /// <summary>
        /// Gets the text focus.
        /// </summary>
        public bool TextFocus {
            get => _textFocus;
            set => Set(ref _textFocus, value);
        }

        public bool IsRunning {
            get => _isRunning;
            set => Set(ref _isRunning, value);
        }

        /// <summary>
        /// Gets the Words to be typed.
        /// </summary>
        public string Words {
            get => _words;
            set => Set(ref _words, value);
        }

        /// <summary>
        /// Get string of removed words after being typed.
        /// </summary>
        public string RemovedWords {
            get => _removedWords;
            set => Set(ref _removedWords, value);
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
        /// Flag to determine to start in learn mode.
        /// </summary>
        public bool IsLearnMode {
            get => _learnMode;
            set => Set(ref _learnMode, value);
        }

        /// <summary>
        /// Instantiate new SpeedViewModel.
        /// </summary>
        /// <param name="typingProfilerFactory"></param>
        /// <param name="messengerHub"></param>
        /// <param name="dataSerializer"></param>
        /// <param name="settingsRepository"></param>
        /// <param name="jsonLoader"></param>
        public SpeedViewModel(ITypingProfilerFactory typingProfilerFactory, ITinyMessengerHub messengerHub, IDataSerializer dataSerializer, ISettingsRepository settingsRepository, IJSonLoader jsonLoader) {
            _model = new SpeedModel(typingProfilerFactory);
            _dataSerializer = dataSerializer;
            _settingsRepository = settingsRepository;
            _jsonLoader = jsonLoader;
            StartTestCommand = new RelayCommand(StartTest, StartTestCanExecute);
            Document = new FlowDocument { FontSize = 40, FontFamily = new FontFamily("Segoe UI"), PagePadding = new Thickness(0)};
            IsRunning = false;
            messengerHub.Subscribe<TestCompleteMessage>(TestCompleteAction);
            LoadSettings();
            _settingsRepository.SettingsUpdatedEvent += SettingsRepositoryOnSettingsUpdatedEvent;
        }

        /// <summary>
        /// Refresh settings on the update event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsRepositoryOnSettingsUpdatedEvent(object sender, System.EventArgs e) {
            foreach (var settingsRepositorySetting in _settingsRepository.Settings) {
                if (settingsRepositorySetting.Name == nameof(IsLearnMode)) {
                    //The more settings addded to this class, add them here for loading.
                    IsLearnMode = (bool)settingsRepositorySetting.Data;
                }
            }
        }

        /// <summary>
        /// Load settings at startup.
        /// </summary>
        private void LoadSettings() {
            foreach (var settingsRepositorySetting in _settingsRepository.Settings) {
                if (settingsRepositorySetting.Name == nameof(IsLearnMode)) {
                    //The more settings addded to this class, add them here for loading.
                    IsLearnMode = (bool) settingsRepositorySetting.Data;
                }
            }
        }

        /// <summary>
        /// Handle event for test complete.
        /// </summary>
        /// <param name="obj"></param>
        private void TestCompleteAction(TestCompleteMessage obj) {
            Words = " ";
            RemovedWords = " ";
            RaisePropertyChanged(nameof(Words));
            CurrentWord = "";
            RaisePropertyChanged(nameof(CurrentWord));
            CurrentChar = "";
            RaisePropertyChanged(nameof(CurrentChar));
            CurrentFinger = "";

            Document.Dispatcher.Invoke(() => {
                Document.Blocks.Clear();
                RaisePropertyChanged(nameof(Document));
            });

           

            //This method is running on a different thread to the UI thread, which the command must run on.
            //To fix this we get the current Dispatcher (UI Thread) and execute the code on there.
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => StartTestCommand.RaiseCanExecuteChanged()));
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData + @"\" + Resources.TypingKataWpmJson;

            if (IsRunning == false) {

                var wpmObjs = _dataSerializer.DeserializeObject<List<WPMJsonObject>>(File.ReadAllText(path));

                if (wpmObjs == null) {
                    wpmObjs = new List<WPMJsonObject>();
                }
                var errorWords = new List<Tuple<string, string>>();
                foreach (var (item1, item2) in obj.Content.ErrorWords) {
                    errorWords.Add(new Tuple<string, string>(item1.ToString(), item2.ToString()));
                }

                var newResult = new WPMJsonObject(obj.Content.Wpm, obj.Content.ErrorWords.Count, obj.Content.ErrorRate,
                    errorWords, DateTime.Now, _testTime);
                wpmObjs.Add(newResult);

                _dataSerializer.SerializeObject(wpmObjs, path);
                _jsonLoader.RefreshJsonFiles();
            }

            TextFocus = false;
            IsRunning = false;
        }
        
        /// <summary>
        /// Predicate to determine if you can start test.
        /// </summary>
        /// <returns></returns>
        private bool StartTestCanExecute() {
            return _testTime >= 60 && IsRunning == false;
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

            Words = string.Join("", TypingProfiler?.GeneratedWords);

            IsRunning = true;
            StartTestCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Create the profiler and subscribe to events.
        /// </summary>
        private void CreateProfiler() {
            var profiler = _model.ConstructProfiler(TestTime);
            profiler.KeyComplete += ProfilerOnKeyComplete;
            profiler.Cursor.CharacterChangedEvent += CursorOnCharacterChangedEvent;
            profiler.NextWordEvent += TypingProfilerOnNextWordEvent;
            profiler.BackspaceCompleteEvent += TypingProfilerOnBackspaceCompleteEvent;

            RaisePropertyChanged(nameof(TypingProfiler));
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

        /// <summary>
        /// Get the inline of the FlowDocument, and remove the last character from it.
        /// </summary>
        private void BackspaceDocumentText() {
            if (!IsRunning) return;
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
        /// Handle next word event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypingProfilerOnNextWordEvent(object sender, WordChangedEventArgs e) {

            var errorDifference = 1;

            //Calculate how much to trim off words in the case that space is pressed early.
            if (!e.IsCorrect) {
                errorDifference = e.NewWord.CharCount - e.OldWord.Chars.Select(x => x.Status).Count(c => c == CharacterStatus.Correct);
            }

            CurrentChar = e.NewWord[0].CurrentCharacter;
            MapCharacterToFinger(CurrentChar);

            if (errorDifference > 0) {
                Words = Words.Remove(0, errorDifference);
            }
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
            MapCharacterToFinger(CurrentChar);
        }

        /// <summary>
        /// Update finger information when the character changes.
        /// </summary>
        /// <param name="c"></param>
        private void MapCharacterToFinger(string c) {
            if (!IsLearnMode) return;
            var currentFinger = FingerColourMapping.FingerMapping[c.ToUpper()];
            CurrentFinger = currentFinger.GetDescription();
            var color = currentFinger.GetColorValue();

            //Setting a brush colour is a dependency property, meaning it must be done on a UI thread.
            //This code makes sure that it's done on the same thread.
            Application.Current.Dispatcher.BeginInvoke(new ThreadStart(() => {
                CurrentFingerColor = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }));
        }

        /// <summary>
        /// Current finger to display based on character.
        /// </summary>
        public string CurrentFinger {
            get => _currentFinger;
            set => Set(ref _currentFinger, value);
        }

        /// <summary>
        /// Current colour to display with finger information.
        /// </summary>
        public Brush CurrentFingerColor {
            get => _currentFingerColor;
            set => Set(ref _currentFingerColor, value);
        }

        /// <summary>
        /// Handle the key complete after typing profiler has processed it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProfilerOnKeyComplete(object sender, KeyInputEventHandlerArgs e) {

            if (!IsRunning) return;

            CurrentWord = TypingProfiler.Cursor.CurrentWord.ToString();
            var isKeyCorrect = e.IsCorrect;
            Log.Debug($"Key pressed : {e.InputKey}");
            var status = isKeyCorrect ? CharacterStatus.Correct : CharacterStatus.Incorrect;

            if (e.InputKey == ' ' || e.InputKey == '\b') {
                RemovedWords += e.InputKey;
                return;
            }

            var tr = new TextRange(Document.ContentEnd, Document.ContentEnd)
                {Text = new string(new[] {e.InputKey})};

            switch (status) {
                case CharacterStatus.Correct:
                    tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.AliceBlue);
                    RemovedWords += e.InputKey;
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