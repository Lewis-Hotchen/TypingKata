using Autofac;
using Autofac.Core;
using KataIocModule;

namespace KataSpeedProfilerModule {
    public class TypingProfiler {

        public ICursor Cursor { get; }
        public IWordStack UserWords { get; }
        public IWordStack ErrorWords { get; }
        public ITypingTimer Timer { get; }


        public TypingProfiler(ICursor cursor,  IWordStack userWords, IWordStack errorWords, ITypingTimer timer) {
            Cursor = cursor;
            UserWords = userWords;
            ErrorWords = errorWords;
            Timer = timer;
        }

        public void StartTest(IWord firstWord, double seconds) {

            
        }

        public void NextWord() {

        }
    }
}
