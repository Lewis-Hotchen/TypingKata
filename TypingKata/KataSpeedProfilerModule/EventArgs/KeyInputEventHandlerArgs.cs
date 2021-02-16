using System.Windows.Input;

namespace KataSpeedProfilerModule.EventArgs {
    public class KeyInputEventHandlerArgs {
        public bool IsCorrect { get; set; }
        public char InputKey { get; set; }

        public KeyInputEventHandlerArgs(bool isCorrect, char inputKey) {
            IsCorrect = isCorrect;
            InputKey = inputKey;
        }
    }
}