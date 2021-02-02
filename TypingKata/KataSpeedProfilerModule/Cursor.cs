using System;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Class that will handle the logical cursor for the typing test
    /// </summary>
    public class Cursor : ICursor {
        private IWord _currentWord;
        private int _wordPos;
        private int _charPos;
        private bool _wordSetCallback;
        private bool _charSetCallback;

        /// <summary>
        /// Event that will fire when a word has been completed.
        /// </summary>
        public event EventHandler WordCompletedEvent;

        /// <summary>
        /// Current cursor word position.
        /// </summary>
        public int WordPos {
            get => _wordPos;
            private set => _wordSetCallback = SetWordPos(value);
        }

        /// <summary>
        /// Current cursor character position.
        /// </summary>
        public int CharPos {
            get => _charPos;
            private set {
                _charSetCallback = SetCharPos(value);
                if (_charPos == CurrentWord.CharCount)
                    WordCompletedEvent?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// The current word.
        /// </summary>
        public IWord CurrentWord {
            get => _currentWord;
            private set => SetWord(value);
        }

        /// <summary>
        /// Set the current word.
        /// </summary>
        /// <param name="value"></param>
        private void SetWord(IWord value) {
            if (value != null) {
                _currentWord = value;
            }
        }

        /// <summary>
        /// Set the new character position.
        /// </summary>
        /// <param name="value">New character position.</param>
        /// <returns>True if successful, otherwise false.</returns>
        private bool SetCharPos(int value) {
            if (value < 0) {
                if (_charPos <= 0)
                    return false;
                _charPos += value;

                return true;
            }

            if (CurrentWord.CharCount < _charPos + value) {

            }

            _charPos += value;

            return true;
        }

        /// <summary>
        /// Set the new word position.
        /// </summary>
        /// <param name="value">The new word position.</param>
        /// <returns>True if successful, otherwise false.</returns>
        private bool SetWordPos(int value) {
            //Check if value is < 0 for decrement
            if (value < 0) {
                if (_wordPos <= 0)
                    return false;
                _wordPos += value;
                return true;
            }

            _wordPos += value;
            return true;
        }

        /// <summary>
        /// Increment character position.
        /// </summary>
        /// <param name="increment">Number to increment by.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public bool NextChar(int increment) {
            CharPos = increment;
            return _charSetCallback;
        }

        /// <summary>
        /// Increment word position.
        /// </summary>
        /// <param name="increment">The increment number.</param>
        /// <param name="word">The new word.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public bool NextWord(int increment, IWord word) {
            WordPos = increment;

            //check if word pos 
            if (_wordSetCallback && word != null) {
                CurrentWord = word;
                _charPos = 0;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Instantiate new cursor with no word.
        /// </summary>
        public Cursor() {
            Setup();
        }

        /// <summary>
        /// Reset the cursor.
        /// </summary>
        public void ResetCursor() {
            CharPos = 0;
            WordPos = 0;
            CurrentWord = null;
        }

        /// <summary>
        /// Instantiate cursor with a word.
        /// </summary>
        /// <param name="firstWord">GeneratedWord to begin with.</param>
        public Cursor(IWord firstWord) {
            Setup();
            CurrentWord = firstWord;
        }

        /// <summary>
        /// Setup the cursor.
        /// </summary>
        private void Setup() {
            _wordPos = 0;
            _charPos = 0;
            _wordSetCallback = false;
            _charSetCallback = false;
        }
    }
}