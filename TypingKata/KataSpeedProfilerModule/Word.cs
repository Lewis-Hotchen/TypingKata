namespace KataSpeedProfilerModule {
    public class Word : IWord {
        public char[] Chars { get; }
        public int CharCount => Chars.Length;

        public Word(string word) {
            Chars = word.ToCharArray();
        }

        public override string ToString() {
            return new string(Chars);
        }
    }
}