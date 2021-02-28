using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple factory to create TypingProfiler.
    /// </summary>
    public interface ITypingProfilerFactory {
        ITypingProfiler CreateTypingProfiler(ICursor cursor, 
            IWordStack userWords, 
            ITypingTimer timer,
            IMarkovChainGenerator generator);
    }
}