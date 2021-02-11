namespace KataSpeedProfilerModule.EventArgs {
    public class CharacterChangedEventArgs {

        public char OldValue { get; }
        public char NewValue { get; }

        public CharacterChangedEventArgs(char oldValue, char newValue) {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}