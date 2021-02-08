using System.Collections.Generic;

namespace KataSpeedProfilerModule.Interfaces {

    /// <summary>
    /// Interface to store a word.
    /// </summary>
    public interface IWord {
        /// <summary>
        /// The list of characters in the word.
        /// </summary>
        IList<char> Chars { get; }

        /// <summary>
        /// Get characters from the word by index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The character.</returns>
        char this[int index] { get; }

        /// <summary>
        /// The count of characters in the word.
        /// </summary>
        int CharCount { get; }
    }
}