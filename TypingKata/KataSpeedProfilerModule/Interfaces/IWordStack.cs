using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {
    public interface IWordStack {

        /// <summary>
        /// Push new word into the stack.
        /// </summary>
        /// <param name="word">The word to be pushed.</param>
        void Push(IWord word);

        /// <summary>
        /// Remove top word from stack.
        /// </summary>
        /// <returns>Return true if successfully removed from stack; otherwise, return false.</returns>
        bool Pop();

        /// <summary>
        /// Get the stack as an array.
        /// </summary>
        /// <returns></returns>
        IWord[] GetWordsAsArray();

        /// <summary>
        /// GeneratedWord at the top of the stack. Acts as current word from <see cref="Cursor"/>
        /// </summary>
        IWord Top { get; }

        /// <summary>
        /// The current count of words in the stack.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clear the stack.
        /// </summary>
        void ClearStack();
    }
}