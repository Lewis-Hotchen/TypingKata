using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace KataSpeedProfilerModule {
    public class GeneratedWord : IWord {
        public IList<char> Chars { get; }
        public int CharCount => Chars.Count();

        public GeneratedWord(string word) {
            Chars = word.ToCharArray();
        }

        public override string ToString() {
            return new string(Chars.ToArray());
        }
    }
}