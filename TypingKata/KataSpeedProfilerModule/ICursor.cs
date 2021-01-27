using System;

namespace KataSpeedProfilerModule {
    public interface ICursor {
        int WordPos { get; }
        int CharPos { get; }
        IWord CurrentWord { get; }
        event EventHandler WordCompletedEvent;
        bool NextChar(int increment);
        bool NextWord(int increment, IWord word);
        void ResetCursor();
    }
}