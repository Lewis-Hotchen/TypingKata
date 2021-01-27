namespace KataSpeedProfilerModule {
    public interface IWordStack {
        void Push(IWord word);
        bool Pop();
        IWord[] GetWordsAsArray();
        IWord Top { get; }
        int Count { get; }
        void ClearStack();
    }
}