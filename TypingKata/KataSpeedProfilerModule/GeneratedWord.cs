using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple class to store a generated word.
    /// </summary>
    public class GeneratedWord : IWord {

        /// <summary>
        /// The list of characters in the word.
        /// </summary>
        public IList<char> Chars { get; }

        public char this[int index] => Chars[index];

        /// <summary>
        /// The count of characters in the word.
        /// </summary>
        public int CharCount => Chars.Count();

        /// <summary>
        /// Instantiates new GeneratedWord class.
        /// </summary>
        /// <param name="word"></param>
        public GeneratedWord(string word) {
            Chars = word.ToCharArray();
        }

        /// <summary>
        /// Return the characters as a string word.
        /// </summary>
        /// <returns>String of characters.</returns>
        public override string ToString() {
            return new string(Chars.ToArray());
        }
    }
}