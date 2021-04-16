using Autofac;
using Autofac.Core;
using KataIocModule;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple factory to create TypingProfiler.
    /// </summary>
    public class TypingProfilerFactory : ITypingProfilerFactory {

        /// <summary>
        /// Creates a typing profiler.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="typingSpeedCalculator"></param>
        /// <param name="timer"></param>
        /// <param name="messengerHub"></param>
        /// <returns></returns>
        public ITypingProfiler CreateTypingProfiler(ICursor cursor, ITypingSpeedCalculator typingSpeedCalculator, ITypingTimer timer,
            ITinyMessengerHub messengerHub) {
            return BootStrapper.Resolve<ITypingProfiler>(new Parameter[] {
                new NamedParameter("cursor", cursor),
                new NamedParameter("typingSpeedCalculator", typingSpeedCalculator),
                new NamedParameter("timer", timer), 
                new NamedParameter("messengerHub", messengerHub),
            });
        }
    }
}