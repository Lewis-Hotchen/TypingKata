using System.Collections.Generic;
using KataSpeedProfilerModule.EventArgs;

namespace KataSpeedProfilerModule.Interfaces {
    /// <summary>
    /// Interface for Typing Speed Calculator.
    /// </summary>
    public interface ITypingSpeedCalculator {
        WordChangedEventArgs CompareAndCommitWords();
        void ResetCalculator();
        TestCompleteMessage CalculateWpm(int time);
        IEnumerable<string> GenerateWords(int minutes);

        List<(IWord, IWord)> ErrorWords { get; }
        LinkedList<IWord> GeneratedWords { get; }
        LinkedList<IWord> RemovedWords { get; }
        IWordStack UserWords { get; }
    }
}