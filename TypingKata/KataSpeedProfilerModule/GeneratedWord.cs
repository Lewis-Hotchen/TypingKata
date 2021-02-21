using System.Collections.Generic;
using System.Linq;
using System.Text;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple class to store a generated word.
    /// </summary>
    public class GeneratedWord : IWord {

        /// <summary>
        /// The list of characters in the word.
        /// </summary>
        public List<(char, CharacterStatus)> Chars { get; }

        /// <summary>
        /// Get element by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public (char, CharacterStatus) this[int index] => Chars[index];

        /// <summary>
        /// The count of characters in the word.
        /// </summary>
        public int CharCount => Chars.Count();

        /// <summary>
        /// Instantiates new GeneratedWord class.
        /// </summary>
        /// <param name="word"></param>
        public GeneratedWord(string word) {
            Chars = new List<(char, CharacterStatus)>();
            foreach (var t in word) {
                Chars.Add((t, CharacterStatus.Unmodified));
            }
        }

        /// <summary>
        /// Return the characters as a string word.
        /// </summary>
        /// <returns>String of characters.</returns>
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            foreach (var c in Chars) {
                sb.Append(c.Item1);
            }

            return sb.ToString();
        }
    }
}