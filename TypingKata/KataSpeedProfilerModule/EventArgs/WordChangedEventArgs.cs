using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule.EventArgs {
    public class WordChangedEventArgs {
        public IWord OldWord { get; }
        public IWord NewWord { get; }

        public WordChangedEventArgs(IWord oldWord, IWord newWord) {
            OldWord = oldWord;
            NewWord = newWord;
        }
    }
}