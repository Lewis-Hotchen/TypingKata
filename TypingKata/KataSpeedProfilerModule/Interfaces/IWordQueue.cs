using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple interface that wraps a C# Queue.
    /// </summary>
    public interface IWordQueue {

        /// <summary>
        /// The top of the queue, or first in queue.
        /// </summary>
         IWord Top { get; }

        /// <summary>
        /// The count of words in the queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get the queue as an array.
        /// </summary>
        /// <returns></returns>
        IWord[] GetWordQueueAsArray();

        /// <summary>
        /// Clear the queue.
        /// </summary>
        void ClearQueue();

        /// <summary>
        /// Add word to the queue.
        /// </summary>
        /// <param name="word">The word to be added.</param>
        void Enqueue(IWord word);

        /// <summary>
        /// Remove word from the queue.
        /// </summary>
        /// <returns>The word to be removed.</returns>
        IWord Dequeue();
    }
}