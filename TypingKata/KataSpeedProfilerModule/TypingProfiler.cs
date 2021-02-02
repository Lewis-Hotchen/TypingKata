using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Autofac;
using Autofac.Core;
using KataIocModule;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Class that will handle the typing test.
    /// </summary>
    public class TypingProfiler : ITypingProfiler {

        public ICursor Cursor { get; }
        public IWordStack UserWords { get; }
        public IWordStack ErrorWords { get; }
        public ITypingTimer Timer { get; }
        public IWordQueue Queue { get; }
        private bool endOfWord;

        /// <summary>
        /// Instantiate new TypingProfiler.
        /// </summary>
        /// <param name="cursor">The Cursor.</param>
        /// <param name="userWords">The user words.</param>
        /// <param name="errorWords">The error words.</param>
        /// <param name="queue">The word queue.</param>
        /// <param name="timer">The timer.</param>
        public TypingProfiler(ICursor cursor,  IWordStack userWords, IWordStack errorWords, IWordQueue queue, ITypingTimer timer) {
            Cursor = cursor;
            UserWords = userWords;
            ErrorWords = errorWords;
            Queue = queue;
            Timer = timer;
            Setup();
        }

        private void Setup() {
            Cursor.WordCompletedEvent += CursorOnWordCompletedEvent;
            Timer.TimeComplete += TimerOnTimeComplete;
            QueueNewWords();
            endOfWord = false;
        }

        private void TimerOnTimeComplete(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When the cursor signals the word is complete, we move on to the next word logically in the
        /// word queue, by dequeuing the last word and passing in the new top.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CursorOnWordCompletedEvent(object sender, EventArgs e) {
            Queue.Dequeue();
            Cursor.NextWord(1, Queue.Top);
        }

        /// <summary>
        /// Generate words from resource using Markov algorithm.
        /// </summary>
        /// <returns>Enumerable string of words.</returns>
        private IEnumerable<string> GetWordsFromResource() {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "KataSpeedProfilerModule.Resources.words.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException())) {
                var result = reader.ReadToEnd();
                var words = MarkovChainTextGenerator.Markov(result.Split(
                    new[] { Environment.NewLine },
                    StringSplitOptions.None
                ), 3, 50);

                return words.Split(' ');
            }
        }

        /// <summary>
        /// Start the test.
        /// </summary>
        public void StartTest() {

        }

        public bool CharacterInput(Key key) {
            var currWord = Cursor.CurrentWord;
            if (key == Key.Space) {
                return endOfWord;
            }

            if (currWord.Chars[Cursor.CharPos] == key.ToString().ToCharArray()[0]) {
                Cursor.NextChar(1);
                UserWords.Top.Chars.Add(key.ToString().ToCharArray()[0]);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Queue up new words from Markov generation.
        /// </summary>
        private void QueueNewWords() {
            var words = GetWordsFromResource();
            foreach (var word in words) {
                Queue.Enqueue(BootStrapper.Resolve<IWord>(new Parameter[]{new NamedParameter("word", word)}));
            }
        }
    }
}