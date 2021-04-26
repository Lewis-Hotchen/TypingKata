using System.Collections.Generic;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule.EventArgs {
    public class TestCompleteEventArgs {

        public double ErrorRate { get; }
        public List<(IWord, IWord)> ErrorWords { get; }
        public int Wpm { get; }
        public double Time { get; }

        public TestCompleteEventArgs(double errorRate, List<(IWord, IWord)> errorWords, int wpm, double time) {
            ErrorRate = errorRate;
            ErrorWords = errorWords;
            Wpm = wpm;
            Time = time;
        }

    }
}