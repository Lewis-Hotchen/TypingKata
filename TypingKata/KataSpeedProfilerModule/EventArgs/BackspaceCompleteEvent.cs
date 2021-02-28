namespace KataSpeedProfilerModule.EventArgs {
    public class BackspaceCompleteEvent {
        public char AddedChar { get; }
        public CharacterStatus IsError { get; }

        public BackspaceCompleteEvent(char c, CharacterStatus isError) {
            AddedChar = c;
            IsError = isError;
        }
    }
}