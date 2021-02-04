using System.Collections.Generic;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Interface to store a word.
    /// </summary>
    public interface IWord {
        /// <summary>
        /// The list of characters in the word.
        /// </summary>
        IList<char> Chars { get; }

        /// <summary>
        /// The count of characters in the word.
        /// </summary>
        int CharCount { get; }
    }
}