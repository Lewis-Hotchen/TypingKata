using System;
using System.Collections.Generic;
using KataSpeedProfilerModule.EventArgs;

namespace KataSpeedProfilerModule.Interfaces {

    /// <summary>
    /// This class will handle the
    /// initiation and tracking of the typing test.
    /// </summary>
    public interface ITypingProfiler {

        ICursor Cursor { get; }
        string[] GeneratedWords { get; }
        void CharacterInput(char key);
        void Start();
        event EventHandler<KeyInputEventHandlerArgs> KeyComplete;
        event EventHandler<WordChangedEventArgs> NextWordEvent;
        event EventHandler<BackspaceCompleteEvent> BackspaceCompleteEvent;
    }
}