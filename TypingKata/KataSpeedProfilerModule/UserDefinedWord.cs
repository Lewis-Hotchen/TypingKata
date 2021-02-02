using System.Collections.Generic;
using System.Linq;

namespace KataSpeedProfilerModule {
    class UserDefinedWord : IWord {
        public IList<char> Chars { get; }
        public int CharCount { get; }

        public override string ToString() {
            return new string(Chars.ToArray());
        }

        public UserDefinedWord() {
        }
    }
}