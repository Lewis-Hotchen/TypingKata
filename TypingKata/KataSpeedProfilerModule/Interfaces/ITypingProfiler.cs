using System;
using KataSpeedProfilerModule.EventArgs;

namespace KataSpeedProfilerModule.Interfaces {

    /// <summary>
    /// This class will handle the
    /// initiation and tracking of the typing test.
    /// </summary>
    public interface ITypingProfiler {

        /// <summary>
        /// The Cursor.
        /// </summary>
        ICursor Cursor { get; }

        /// <summary>
        /// Collection of all the generated words created on start.
        /// </summary>
        string[] GeneratedWords { get; }

        /// <summary>
        /// Send input to the typing test for processing.
        /// </summary>
        /// <param name="key">The character pressed.</param>
        void CharacterInput(char key);

        /// <summary>
        /// Start the test.
        /// </summary>
        void Start();

        /// <summary>
        /// Abort the test early. Test results are not saved.
        /// </summary>
        void AbortTest();
        event EventHandler<KeyInputEventHandlerArgs> KeyComplete;
        event EventHandler<WordChangedEventArgs> NextWordEvent;
    }
}