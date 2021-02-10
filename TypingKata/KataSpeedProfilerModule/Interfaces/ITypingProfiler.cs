using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace KataSpeedProfilerModule.Interfaces {

    /// <summary>
    /// This class will handle the
    /// initiation and tracking of the typing test.
    /// </summary>
    public interface ITypingProfiler {

        ICursor Cursor { get; }
        IWordStack UserWords { get; }
        IList<(IWord, IWord)> ErrorWords { get; }
        ITypingTimer Timer { get; }
        IWordQueue Queue { get; }
        void CharacterInput(Key key);
        void Start(int keySize, int outputSize);
        event EventHandler<KeyInputEventHandlerArgs> KeyComplete;
    }
}