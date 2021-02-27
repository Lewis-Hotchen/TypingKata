namespace KataSpeedProfilerModule {
    public class CharacterDescriptor {
        public string CurrentCharacter { get; set; }
        public CharacterStatus Status { get; set; }

        public CharacterDescriptor(string currentCharacter, CharacterStatus status) {
            CurrentCharacter = currentCharacter;
            Status = status;
        }
    }
}