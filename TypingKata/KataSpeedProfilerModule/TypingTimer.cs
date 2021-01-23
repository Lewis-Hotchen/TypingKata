using System;
using System.Timers;

namespace KataSpeedProfilerModule {
    public class TypingTimer : ITypingTimer {

        private readonly Timer _timer;
        public event EventHandler TimeComplete;

        /// <summary>
        /// Instantiates new TypingTimer.
        /// </summary>
        /// <param name="time">Time in seconds.</param>
        public TypingTimer(double time) {
            _timer = new Timer(time * 1000);
            _timer.Elapsed += TimerOnElapsed;

            //Make sure that the timer elapsed will only fire once.
            _timer.AutoReset = false;
        }

        /// <summary>
        /// Event raised when the timer elapsed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnElapsed(object sender, ElapsedEventArgs e) {
            TimeComplete?.Invoke(this, new EventArgs());
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