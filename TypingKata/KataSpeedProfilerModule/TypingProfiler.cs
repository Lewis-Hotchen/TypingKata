using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Autofac;
using KataIocModule;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// This class will handle the
    /// initiation and tracking of the typing test.
    /// </summary>
    public class TypingProfiler : ITypingProfiler {

        public ICursor Cursor { get; }
        public IWordStack UserWords { get; }
        public IList<(IWord, IWord)> ErrorWords { get; }
        public ITypingTimer Timer { get; }
        public IWordQueue Queue { get; }
        public event EventHandler<KeyInputEventHandlerArgs> KeyComplete;

        /// <summary>
        /// Instantiate new TypingProfiler.
        /// </summary>
        /// <param name="cursor">The Cursor.</param>
        /// <param name="userWords">The user words.</param>
        /// <param name="queue">The word queue.</param>
        /// <param name="timer">The timer.</param>
        public TypingProfiler(ICursor cursor,  IWordStack userWords, IWordQueue queue, ITypingTimer timer) {
            Cursor = cursor;
            UserWords = userWords;
            ErrorWords = new List<(IWord, IWord)>();
            Queue = queue;
            Timer = timer;
            Setup();
        }

        /// <summary>
        /// Setup the class.
        /// </summary>
        private void Setup() {
            Cursor.WordCompletedEvent += CursorOnWordCompletedEvent;
            Timer.TimeComplete += TimerOnTimeComplete;
        }

        /// <summary>
        /// Stop the test when the timer is complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnTimeComplete(object sender, EventArgs e) {
            StopTest();
        }

        /// <summary>
        /// Stop the test and reset values.
        /// </summary>
        private void StopTest() {
            Cursor.ResetCursor();
            Queue.ClearQueue();
        }

        /// <summary>
        /// When the cursor signals the word is complete, we move on to the next word logically in the
        /// word queue, by dequeuing the last word and passing in the new top.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CursorOnWordCompletedEvent(object sender, EventArgs e) {

            var userWord = UserWords.Top;
            var generatedWord = Queue.Top;

            //Add word to Error words if they are not the same.
            if (userWord.ToString() != generatedWord.ToString()) {
                ErrorWords.Add((userWord, generatedWord));
            }

            Queue.Dequeue();
            Cursor.NextWord(1, Queue.Top);
        }

        /// <summary>
        /// Generate words from resource using Markov algorithm.
        /// </summary>
        /// <returns>Enumerable string of words.</returns>
        private IEnumerable<string> GetWordsFromResource(int keySize, int outputSize) {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "KataSpeedProfilerModule.Resources.words.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException())) {
                var result = reader.ReadToEnd();
                var words = MarkovChainTextGenerator.Markov(result.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                ), keySize, outputSize);

                return words.Split(' ');
            }
        }

        /// <summary>
        /// Input a character.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void CharacterInput(Key key) {
            if (key == Key.Space) {
                if (Cursor.IsEndOfWord) {
                    Cursor.NextChar(1);
                    KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                }
            }

            //Change the case as Key.ToString() returns upper case.
            var casedChar = char.ToUpper(Cursor.CurrentWord[Cursor.CharPos]);

            if (casedChar == key.ToString().ToCharArray()[0]) {
                Cursor.NextChar(1);
                UserWords.Top.Chars.Add(Cursor.CurrentWord[Cursor.CharPos]);
                KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                Cursor.NextChar(1);
            }

            KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(false, key));
        }

        public void Start(int keySize, int outputSize) {
            QueueNewWords(keySize, outputSize);
            Cursor.NextWord(0, Queue.Top);
        }

        /// <summary>
        /// Queue up new words from Markov generation.
        /// </summary>
        private void QueueNewWords(int keySize, int outputSize) {
            var words = GetWordsFromResource(keySize, outputSize);
            foreach (var word in words) {
                Queue.Enqueue(BootStrapper.Container.ResolveKeyed<IWord>("Generated", new NamedParameter("word", word)));
            }
        }
    }
}