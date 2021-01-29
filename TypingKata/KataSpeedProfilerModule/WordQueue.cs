using System.Collections.Generic;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple class that wraps a C# Queue.
    /// </summary>
    public class WordQueue : IWordQueue {

        private readonly Queue<IWord> _wordQueue;

        /// <summary>
        /// The top of the queue, or first in queue.
        /// </summary>
        public IWord Top => _wordQueue.Peek();

        /// <summary>
        /// The count of words in the queue.
        /// </summary>
        public int Count => _wordQueue.Count;

        /// <summary>
        /// Instantiate new word queue.
        /// </summary>
        public WordQueue() {
            _wordQueue = new Queue<IWord>();
        }

        /// <summary>
        /// Get the queue as an array.
        /// </summary>
        /// <returns></returns>
        public IWord[] GetWordQueueAsArray() {
            return _wordQueue.ToArray();
        }

        /// <summary>
        /// Clear the queue.
        /// </summary>
        public void ClearQueue() {
            _wordQueue.Clear();
        }

        /// <summary>
        /// Add word to the queue.
        /// </summary>
        /// <param name="word">The word to be added.</param>
        public void Enqueue(IWord word) {
            _wordQueue.Enqueue(word);
        }


        /// <summary>
        /// Remove word from the queue.
        /// </summary>
        /// <returns>The word to be removed.</returns>
        public IWord Dequeue() {
            return _wordQueue.Dequeue();
        }
    }
}