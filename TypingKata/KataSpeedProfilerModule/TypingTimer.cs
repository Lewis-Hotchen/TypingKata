using System;
using System.Timers;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Simple class that wraps a system timer.
    /// </summary>  
    public class TypingTimer : ITypingTimer {

        private readonly Timer _timer;

        /// <summary>
        /// Event that fires on the timer's elapsed event.
        /// </summary>
        public event EventHandler TimeComplete;

        /// <summary>
        /// Gets time of timer, expressed in Milliseconds.
        /// </summary>
        public TimeSpan Time { get; }

        /// <summary>
        /// Instantiates new TypingTimer.
        /// </summary>
        /// <param name="time">Time in seconds.</param>
        public TypingTimer(double time) {
            _timer = new Timer(time * 1000);
            _timer.Elapsed += TimerOnElapsed;
            Time = new TimeSpan(0, 0, 0, (int)time);
            //Make sure that the timer elapsed will only fire once.
            _timer.AutoReset = false;
        }

        /// <summary>
        /// Event raised when the timer elapsed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs e) {
            TimeComplete?.Invoke(this, new System.EventArgs());
            _timer.Stop();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer() {
            _timer.Start();
        }
    }
}