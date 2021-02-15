using System;

namespace KataSpeedProfilerModule.Interfaces {

    /// <summary>
    /// Simple interface that wraps a system timer.
    /// </summary>
    public interface ITypingTimer {

        /// <summary>
        /// Event that fires on the timer's elapsed event.
        /// </summary>
        event EventHandler TimeComplete;

        /// <summary>
        /// Gets time of timer, expressed in Milliseconds.
        /// </summary>
        TimeSpan Time { get; }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        void StartTimer();
    }
}
