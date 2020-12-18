using System;

namespace KataSpeedProfilerModule {

    public class Cursor : ICursor {
        private IWord _currentWord;
        private int _wordPos;
        private int _charPos;
        private bool _wordSetCallback;
        private bool _charSetCallback;

        public event EventHandler WordCompletedEvent;

        public int WordPos {
            get => _wordPos;
            private set => _wordSetCallback = SetWordPos(value);
        }

        public int CharPos {
            get => _charPos;
            private set {
                _charSetCallback = SetCharPos(value);
                if (_charPos == CurrentWord.CharCount)
                    WordCompletedEvent?.Invoke(this, new EventArgs());
            }
        }

        public IWord CurrentWord {
            get => _currentWord;
            private set => SetWord(value);
        }

        private void SetWord(IWord value) {
            if (value != null) {
                _currentWord = value;
            }
        }

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

        public bool NextChar(int increment) {
            CharPos = increment;
            return _charSetCallback;
        }

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

        public Cursor() {
            Setup();
        }

        public Cursor(IWord firstWord) {
            Setup();
            CurrentWord = firstWord;
        }

        private void Setup() {
            _wordPos = 0;
            _charPos = 0;
            _wordSetCallback = false;
            _charSetCallback = false;
        }
    }
}