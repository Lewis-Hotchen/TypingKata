using System.Collections.Generic;
using System.Linq;

namespace KataSpeedProfilerModule {
    public class UserDefinedWord : IWord {

        /// <summary>
        /// The list of characters in the word.
        /// </summary>
        public IList<char> Chars { get; }

        /// <summary>
        /// The count of characters in the word.
        /// </summary>
        public int CharCount { get; }

        /// <summary>
        /// Return a string of the list of characters.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return new string(Chars.ToArray());
        }

        /// <summary>
        /// Instantiate new UserDefinedWord.
        /// </summary>
        public UserDefinedWord() {
        }
    }
}