namespace KataSpeedProfilerModule.CharacterMVVM {
    public class CharacterModel {
        public char CurrentCharacter { get; set; }
        public CharacterStatus Status { get; set; }

        /// <summary>
        /// Default constructor 
        /// </summary>
        public CharacterModel() {
            CurrentCharacter = default(char);
            Status = CharacterStatus.Unmodified;
        }

        public CharacterModel(char currentCharacter, CharacterStatus status) {
            CurrentCharacter = currentCharacter;
            Status = status;
        }
    }
}
