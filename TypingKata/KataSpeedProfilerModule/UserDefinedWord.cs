using System.Collections.Generic;
using System.Text;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {
    public class UserDefinedWord : IWord {

        /// <summary>
        /// The list of characters in the word.
        /// </summary>
        public List<CharacterDescriptor> Chars { get; }

        public CharacterDescriptor this[int index] => Chars[index];

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
                sb.Append(c.CurrentCharacter);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Instantiate new UserDefinedWord.
        /// </summary>
        public UserDefinedWord() {
            Chars = new List<CharacterDescriptor>();
        }
    }
}