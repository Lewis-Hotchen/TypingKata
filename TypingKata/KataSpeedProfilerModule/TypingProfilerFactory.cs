using Autofac;
using Autofac.Core;
using KataIocModule;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple factory to create TypingProfiler.
    /// </summary>
    public class TypingProfilerFactory : ITypingProfilerFactory {

        /// <summary>
        /// Create typing profiler.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="userWords">The User words stack.</param>
        /// <param name="queue">The generated words queue.</param>
        /// <param name="timer">The Timer.</param>
        /// <returns>New ITypingProfiler.</returns>
        public ITypingProfiler CreateTypingProfiler(ICursor cursor, IWordStack userWords, IWordQueue queue, ITypingTimer timer) {
            return BootStrapper.Resolve<ITypingProfiler>(new Parameter[] {
                new NamedParameter("cursor", cursor),
                new NamedParameter("userWords", userWords),
                new NamedParameter("queue", queue),
                new NamedParameter("timer", timer), 
            });
        }
    }
}