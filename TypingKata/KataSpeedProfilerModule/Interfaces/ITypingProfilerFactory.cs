using KataIocModule;

namespace KataSpeedProfilerModule.Interfaces {

    /// <summary>
    /// Simple factory to create TypingProfiler.
    /// </summary>
    public interface ITypingProfilerFactory {
        ITypingProfiler CreateTypingProfiler(
            ICursor cursor, 
            ITypingSpeedCalculator typingSpeedCalculator, 
            ITypingTimer timer,
            ITinyMessengerHub messengerHub);
    }
}