using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Autofac;
using KataIocModule;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;
using MarkVSharp;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// This class will handle the
    /// initiation and tracking of the typing test.
    /// </summary>
    public class TypingProfiler : ITypingProfiler {

        private readonly IMarkovChainGenerator _markovChainGenerator;
        public ICursor Cursor { get; }
        public IWordStack UserWords { get; }
        public List<(IWord, IWord)> ErrorWords { get; }
        public ITypingTimer Timer { get; }
        public IWordQueue Queue { get; }
        public event EventHandler<KeyInputEventHandlerArgs> KeyComplete;
        public event EventHandler<CharacterChangedEventArgs> NextWordEvent;
        public event EventHandler<TestCompleteEventArgs> TestCompleteEvent;

        /// <summary>
        /// Instantiate new TypingProfiler.
        /// </summary>
        /// <param name="cursor">The Cursor.</param>
        /// <param name="userWords">The user words.</param>
        /// <param name="queue">The word queue.</param>
        /// <param name="timer">The timer.</param>
        /// <param name="markovChainGenerator"></param>
        public TypingProfiler(ICursor cursor,  IWordStack userWords, IWordQueue queue, ITypingTimer timer, IMarkovChainGenerator markovChainGenerator) {
            _markovChainGenerator = markovChainGenerator;
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
            Timer.TimeComplete += TimerOnTimeComplete;
        }

        /// <summary>
        /// Stop the test when the timer is complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnTimeComplete(object sender, System.EventArgs e) {
            StopTest();
        }

        /// <summary>
        /// Stop the test and reset values.
        /// </summary>
        private void StopTest() {
            Cursor.ResetCursor();
            Queue.ClearQueue();

            //Write out user stack and error stack.

            CalculateWpm();
        }

        private void CalculateWpm() {
            var words = UserWords.GetWordsAsArray();
            var noOfCharsTyped = words.Sum(x => x.CharCount);
            var grossWpm = Convert.ToInt32((noOfCharsTyped / 5) / Timer.Time.Minutes);
            var wpm = grossWpm - (ErrorWords.Count / Timer.Time.Minutes);
            var errorRate = ((double) UserWords.Count / 100) * ErrorWords.Count;

            //Signal that test has stopped.
            TestCompleteEvent?.Invoke(this, new TestCompleteEventArgs(errorRate, ErrorWords, wpm));
        }

        private void ConfirmSpace() {
            var userWord = UserWords.Top;
            var generatedWord = Queue.Top;

            //Add word to Error words if they are not the same.
            if (userWord.ToString() != generatedWord.ToString()) {
                ErrorWords.Add((userWord, generatedWord));
            }

            Queue.Dequeue();
            UserWords.Push(new UserDefinedWord());
            Cursor.NextWord(1, Queue.Top);
            NextWordEvent?.Invoke(this, new CharacterChangedEventArgs(default(char), Cursor.CurrentWord[0]));
        }

        /// <summary>
        /// Input a character.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void CharacterInput(Key key) {
            if (key == Key.Space) {
                if (Cursor.IsEndOfWord) {
                    ConfirmSpace();
                    KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                }
            }

            //Change the case as Key.ToString() returns upper case.
            var casedChar = char.ToUpper(Cursor.CurrentWord[Cursor.CharPos]);

            if (casedChar == key.ToString().ToCharArray()[0]) {
                Cursor.NextChar(1);
                UserWords.Top.Chars.Add(Cursor.CurrentWord[Cursor.CharPos]);
                KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                return;
            }

            KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(false, key));
        }

        public void Start() {
            QueueNewWords();
            Cursor.NextWord(0, Queue.Top);
            Timer.StartTimer();
        }

        /// <summary>
        /// Queue up new words from Markov generation.
        /// </summary>
        private void QueueNewWords() {
            var splitWords =_markovChainGenerator.GetText(20).Split(' ');
            foreach (var word in splitWords) {
                Queue.Enqueue(BootStrapper.Container.ResolveKeyed<IWord>("Generated", new NamedParameter("word", word + " ")));
            }
        }
    }
}