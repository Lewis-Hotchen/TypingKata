using System.Collections.Generic;

namespace KataSpeedProfilerModule {
    public interface IWord {
        IList<char> Chars { get; }
        int CharCount { get; }
    }
}