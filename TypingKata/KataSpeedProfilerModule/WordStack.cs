using System.Collections.Generic;

namespace KataSpeedProfilerModule {
    public class WordStack : IWordStack {
        private readonly Stack<IWord> _words;

        public IWord Top => _words.Peek();
        public int Count => _words.Count;

        public WordStack() {
            _words = new Stack<IWord>();
        }

        public void Push(IWord word) {
            _words.Push(word);
        }

        public IWord[] GetWordsAsArray() {
            return _words.ToArray();
        }

        public void ClearStack() {
            _words.Clear();
        }

        /// <summary>
        /// remove top word from stack.
        /// </summary>
        /// <returns>Return true if successfully removed from stack; otherwise, return false.</returns>
        public bool Pop() {
            var res = _words.Pop();
            return res != null;
        }
    }
}
