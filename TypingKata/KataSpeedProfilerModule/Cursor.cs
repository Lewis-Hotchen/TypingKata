using System;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;

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
        /// Event that will fire when a character is changed.
        /// </summary>
        public event EventHandler<CharacterChangedEventArgs> CharacterChangedEvent;

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

                if (_charSetCallback && CharPos != 0) {
                    CharacterChangedEvent?.Invoke(this, new CharacterChangedEventArgs(CurrentWord[_charPos -1], CurrentWord[_charPos]));
                }
            }
        }

        public bool IsEndOfWord => CharPos == CurrentWord.CharCount - 1;

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
            if (CurrentWord == null) {
                return false;
            }

            if (value < 0) {
                if (_charPos <= 0)
                    return false;
                _charPos += value;

                return true;
            }

            if (_charPos + value <= CurrentWord.CharCount - 1) {
                 _charPos += value;
                 return true;
            }

            return false;
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
            if (CurrentWord == null && WordPos == 0) {
                CurrentWord = word;
                return true;
            }

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
            ResetCursor();
        }

        /// <summary>
        /// Reset the cursor.
        /// </summary>
        public void ResetCursor() {
            CharPos = 0;
            WordPos = 0;
            CurrentWord = null;
            _wordSetCallback = false;
            _charSetCallback = false;
        }
    }
}