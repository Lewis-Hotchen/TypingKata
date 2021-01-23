using System;

namespace KataSpeedProfilerModule {

    public interface ITypingTimer {
        event EventHandler TimeComplete;
        void StartTimer();
    }
}
