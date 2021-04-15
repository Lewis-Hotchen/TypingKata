using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule.EventArgs {
    public class WordChangedEventArgs {
        public IWord OldWord { get; }
        public IWord NewWord { get; }
        public bool IsCorrect { get; }

        public WordChangedEventArgs(IWord oldWord, IWord newWord, bool isCorrect) {
            OldWord = oldWord;
            NewWord = newWord;
            IsCorrect = isCorrect;
        }
    }
}