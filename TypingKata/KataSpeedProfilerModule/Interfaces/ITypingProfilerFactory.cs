using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple factory to create TypingProfiler.
    /// </summary>
    public interface ITypingProfilerFactory {
        ITypingProfiler CreateTypingProfiler(ICursor cursor, 
            IWordStack userWords, 
            IWordQueue queue,
            ITypingTimer timer,
            IMarkovChainGenerator generator);
    }
}