using System;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple interface that wraps a system timer.
    /// </summary>
    public interface ITypingTimer {

        /// <summary>
        /// Event that fires on the timer's elapsed event.
        /// </summary>
        event EventHandler TimeComplete;

        /// <summary>
        /// Starts the timer.
        /// </summary>
        void StartTimer();
    }
}
