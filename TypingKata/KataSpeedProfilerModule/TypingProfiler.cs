using System;
using System.Linq;
using System.Security.RightsManagement;
using KataIocModule;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;
using log4net;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// This class will handle the
    /// initiation and tracking of the typing test.
    /// </summary>
    public class TypingProfiler : ITypingProfiler {
        private readonly ITypingSpeedCalculator _typingSpeedCalculator;
        private readonly ITinyMessengerHub _messengerHub;
        private readonly ILog _log = LogManager.GetLogger(nameof(TypingProfiler));
        private bool _isRunning = false;
        public string[] GeneratedWords { get; private set; }
        public ICursor Cursor { get; }
        public ITypingTimer Timer { get; }
        public event EventHandler<KeyInputEventHandlerArgs> KeyComplete;
        public event EventHandler<WordChangedEventArgs> NextWordEvent;
        public event EventHandler<BackspaceCompleteEvent> BackspaceCompleteEvent;

        /// <summary>
        /// Instantiate new TypingProfiler.
        /// </summary>
        /// <param name="cursor">The Cursor.</param>
        /// <param name="typingSpeedCalculator"></param>
        /// <param name="timer">The timer.</param>
        /// <param name="messengerHub">The tiny messenger hub.</param>
        public TypingProfiler(ICursor cursor,
            ITypingSpeedCalculator typingSpeedCalculator,
            ITypingTimer timer,
            ITinyMessengerHub messengerHub) {
            _typingSpeedCalculator = typingSpeedCalculator;
            _messengerHub = messengerHub;
            Cursor = cursor;
            Timer = timer;
            Setup();
        }

        /// <summary>
        /// Setup the class.
        /// </summary>
        private void Setup() {
            Timer.TimeComplete += TimerOnTimeComplete;
        }

        /// <summary>
        /// Stop the test when the timer is complete.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void TimerOnTimeComplete(object sender, System.EventArgs e) {
            StopTest();
        }

        /// <summary>
        /// Abort the test early. Test results are not saved.
        /// </summary>
        public void AbortTest() {
            _log.Warn("Test was aborted");
            _typingSpeedCalculator.ResetCalculator();
            Cursor.ResetCursor();
            _isRunning = false;
        }

        /// <summary>
        /// Stop the test and reset values.
        /// </summary>
        private void StopTest() {
            //Calculate wpm
            CalculateWpm();

            _typingSpeedCalculator.ResetCalculator();
            Cursor.ResetCursor();
            _isRunning = false;
        }

        /// <summary>
        /// Calculate the wpm.
        /// </summary>
        private void CalculateWpm() {
            var res = _typingSpeedCalculator.CalculateWpm((int) Timer.Time.TotalMinutes);
            _messengerHub.Publish(res);
        }

        /// <summary>
        /// Handle backspace input.
        /// </summary>
        private void HandleBackspace() {
            if (Cursor.WordPos == 0 && Cursor.CharPos == 0) return;

            var userWord = _typingSpeedCalculator.UserWords.Top;

            if (userWord.CharCount == 0) {
                //go back a word
                _typingSpeedCalculator.HandleChangeWordOnBackspace();
                Cursor.NextWord(-1, _typingSpeedCalculator.GeneratedWords.First.Value);
                Cursor.NextChar(Cursor.CurrentWord.CharCount - 1);
                BackspaceCompleteEvent?.Invoke(this, new BackspaceCompleteEvent(' ', CharacterStatus.Correct));

            } else if (userWord.CharCount > 0) {
                //go back a character
                var status = _typingSpeedCalculator.HandleBackspace();
                Cursor.NextChar(-1);
                var c = Cursor.CurrentWord[Cursor.CharPos].CurrentCharacter[0];
                BackspaceCompleteEvent?.Invoke(this, new BackspaceCompleteEvent(c, status));
            }
        }

        /// <summary>
        /// Handle spacebar input.
        /// </summary>
        private void ConfirmSpace() {
            var res = _typingSpeedCalculator.CompareAndCommitWords();
            Cursor.NextWord(1, _typingSpeedCalculator.GeneratedWords.First.Value);
            NextWordEvent?.Invoke(this, res);
        }

        /// <summary>
        /// Input a character.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void CharacterInput(char key) {

            if (!_isRunning) return;

                _log.Debug($"Key pressed : {key}");
            if (key == '\b') {
                HandleBackspace();
                KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                return;
            }

            if (key == ' ') {
                if (_typingSpeedCalculator.UserWords.Top.CharCount != 0) {
                    ConfirmSpace();
                    KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                    _log.Debug("Key pressed: space. Was the correct key");
                    return;
                }

                return;
            }

            //Change the case as Key.ToString() returns upper case.
            var casedChar = Cursor.CurrentWord[Cursor.CharPos].CurrentCharacter.ToUpper();
            if (casedChar[0] == char.ToUpper(key)) {
                _typingSpeedCalculator.UserWords.Top.Chars.Add(new CharacterDescriptor(new string(new []{key}), CharacterStatus.Correct));
                Cursor.NextChar(1);
                KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                _log.Debug($"Key pressed: {key}. Was the correct key");
                return;
            }

            _typingSpeedCalculator.UserWords.Top.Chars.Add(new CharacterDescriptor(new string(new[] { key }), CharacterStatus.Incorrect));
            Cursor.NextChar(1);
            KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(false, key));
            _log.Debug($"Key pressed: {key}. Wasn't the correct key. Correct key: {casedChar}");
        }

        /// <summary>
        /// Start the test.
        /// </summary>
        public void Start() {
            //Determine number of words to generate.
            var minutes = Timer.Time.TotalMinutes;
            GeneratedWords = _typingSpeedCalculator.GenerateWords((int) minutes).ToArray();
            
            Cursor.NextWord(0, _typingSpeedCalculator.GeneratedWords.First.Value);
            Timer.StartTimer();
            _log.Info($"Typing test was started with {Timer.Time.TotalMinutes} minutes, and {GeneratedWords.Length} words.");
            _isRunning = true;
        }
    }
}