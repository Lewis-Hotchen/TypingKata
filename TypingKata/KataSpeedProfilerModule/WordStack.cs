using System;
using System.Collections.Generic;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// This class will handle First in last out (FILO) operations, as a stack, for the
    /// user written words.
    /// This class wraps a C# standard stack class with an interface.
    /// </summary>
    public class WordStack : IWordStack {

        private readonly Stack<IWord> _words;

        /// <summary>
        /// GeneratedWord at the top of the stack. Acts as current word from <see cref="Cursor"/>
        /// </summary>
        public IWord Top => _words.Peek();

        /// <summary>
        /// The current count of words in the stack.
        /// </summary>
        public int Count => _words.Count;

        /// <summary>
        /// Instantiate a new WordStack.
        /// </summary>
        public WordStack() {
            _words = new Stack<IWord>();

            //Push one item to start the stack.
            _words.Push(new UserDefinedWord());
        }

        /// <summary>
        /// Push new word into the stack.
        /// </summary>
        /// <param name="word">The word to be pushed.</param>
        public void Push(IWord word) {
            if (word == null) throw new ArgumentNullException(nameof(word));
            _words.Push(word);
        }

        /// <summary>
        /// Get the stack as an array.
        /// </summary>
        /// <returns></returns>
        public IWord[] GetWordsAsArray() {
            return _words.ToArray();
        }

        /// <summary>
        /// Clear the stack.
        /// </summary>
        public void ClearStack() {
            _words.Clear();
        }

        /// <summary>
        /// Remove top word from stack.
        /// </summary>
        /// <returns>Return true if successfully removed from stack; otherwise, return false.</returns>
        public bool Pop() {
            var res = _words.Pop();
            return res != null;
        }
    }
}