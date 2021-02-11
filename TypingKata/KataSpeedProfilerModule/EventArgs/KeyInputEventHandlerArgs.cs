using System.Windows.Input;

namespace KataSpeedProfilerModule.EventArgs {
    public class KeyInputEventHandlerArgs {
        public bool IsCorrect { get; set; }
        public Key InputKey { get; set; }

        public KeyInputEventHandlerArgs(bool isCorrect, Key inputKey) {
            IsCorrect = isCorrect;
            InputKey = inputKey;
        }
    }
}