using System;
using System.Collections.Generic;
using System.Windows.Input;
using KataSpeedProfilerModule.EventArgs;

namespace KataSpeedProfilerModule.Interfaces {

    /// <summary>
    /// This class will handle the
    /// initiation and tracking of the typing test.
    /// </summary>
    public interface ITypingProfiler {

        ICursor Cursor { get; }
        IWordStack UserWords { get; }
        List<(IWord, IWord)> ErrorWords { get; }
        ITypingTimer Timer { get; }
        LinkedList<IWord> GeneratedWords { get; }
        void CharacterInput(char key);
        void Start(int startingQueueCount);
        event EventHandler<KeyInputEventHandlerArgs> KeyComplete;
        event EventHandler<WordChangedEventArgs> NextWordEvent;
        event EventHandler<TestCompleteEventArgs> TestCompleteEvent;
        event EventHandler<BackspaceComleteEvent> BackspaceCompleteEvent;
    }
}