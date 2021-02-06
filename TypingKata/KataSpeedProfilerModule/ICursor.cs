using System;

namespace KataSpeedProfilerModule {
    /// <summary>
    /// Interface that will handle the logical cursor for the typing test
    /// </summary>
    public interface ICursor {

        /// <summary>
        /// Current cursor word position.
        /// </summary>
        int WordPos { get; }

        /// <summary>
        /// Current cursor character position.
        /// </summary>
        int CharPos { get; }

        /// <summary>
        /// Determine if the cursor is at the end of the word.
        /// </summary>
        bool IsEndOfWord { get; }

        /// <summary>
        /// The current word.
        /// </summary>
        IWord CurrentWord { get; }

        /// <summary>
        /// Event that will fire when a word has been completed.
        /// </summary>
        event EventHandler WordCompletedEvent;

        /// <summary>
        /// Increment character position.
        /// </summary>
        /// <param name="increment">Number to increment by.</param>
        /// <returns>True if successful, otherwise false.</returns>
        bool NextChar(int increment);

        /// <summary>
        /// Increment word position.
        /// </summary>
        /// <param name="increment">The increment number.</param>
        /// <param name="word">The new word.</param>
        /// <returns>True if successful, otherwise false.</returns>
        bool NextWord(int increment, IWord word);

        /// <summary>
        /// Reset the cursor.
        /// </summary>
        void ResetCursor();
    }
}