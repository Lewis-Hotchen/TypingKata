using System.Collections.Generic;
using System.Linq;
using System.Text;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {
    public class UserDefinedWord : IWord {

        /// <summary>
        /// The list of characters in the word.
        /// </summary>
        public List<(char, CharacterStatus)> Chars { get; }

        public (char, CharacterStatus) this[int index] => Chars[index];

        /// <summary>
        /// The count of characters in the word.
        /// </summary>
        public int CharCount => Chars.Count;

        /// <summary>
        /// Return a string of the list of characters.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (var c in Chars) {
                sb.Append(c.Item1);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Instantiate new UserDefinedWord.
        /// </summary>
        public UserDefinedWord() {
            Chars = new List<(char, CharacterStatus)>();
        }
    }
}