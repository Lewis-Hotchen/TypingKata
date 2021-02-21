using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using KataIocModule;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// This class will handle the
    /// initiation and tracking of the typing test.
    /// </summary>
    public class TypingProfiler : ITypingProfiler {

        private readonly IMarkovChainGenerator _markovChainGenerator;

        //Choose 200 as an upper bound. Highly unlikely that anyone could type more
        //than 200wpm.
        private int _generatedTextCount;
        public ICursor Cursor { get; }
        public IWordStack UserWords { get; }
        public List<(IWord, IWord)> ErrorWords { get; }
        public ITypingTimer Timer { get; }
        public LinkedList<IWord> Queue { get; }
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
            Queue = new LinkedList<IWord>();
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
            Queue.Clear();

            //Write out user stack and error stack.

            CalculateWpm();
        }

        private void CalculateWpm() {
            var words = UserWords.GetWordsAsArray();
            var noOfCharsTyped = words.Sum(x => x.CharCount);
            var grossWpm = Convert.ToInt32((noOfCharsTyped / 5) / Timer.Time.TotalMinutes);
            var wpm = (int) (grossWpm - (ErrorWords.Count / Timer.Time.TotalMinutes));
            var errorRate = ((double) UserWords.Count / 100) * ErrorWords.Count;

            //Signal that test has stopped.
            TestCompleteEvent?.Invoke(this, new TestCompleteEventArgs(errorRate, ErrorWords, wpm));
        }

        private void ConfirmSpace() {
            var userWord = UserWords.Top;
            var generatedWord = Queue.First;

            //Add word to Error words if they are not the same.
            if (userWord.ToString() != generatedWord.ToString()) {
                ErrorWords.Add((userWord, generatedWord.Value));
            }

            Queue.RemoveFirst();
            UserWords.Push(new UserDefinedWord());
            Cursor.NextWord(1, Queue.First.Value);
            NextWordEvent?.Invoke(this, new CharacterChangedEventArgs(default(char), Cursor.CurrentWord[0].Item1));
        }

        /// <summary>
        /// Input a character.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void CharacterInput(char key) {
            if (key == ' ') {
                if (Cursor.IsEndOfWord) {
                    ConfirmSpace();
                    KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                }
            }

            //Change the case as Key.ToString() returns upper case.
            var casedChar = char.ToUpper(Cursor.CurrentWord[Cursor.CharPos].Item1);

            if (casedChar == char.ToUpper(key)) {
                UserWords.Top.Chars.Add((key, CharacterStatus.Correct));
                Cursor.NextChar(1);
                KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                return;
            }

            KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(false, key));
        }

        public void Start() {
            
            //Determine number of words to generate.
            var minutes = Timer.Time.TotalMinutes;
            _generatedTextCount = (int) (200 * minutes);

            QueueNewWords();
            Cursor.NextWord(0, Queue.First.Value);
            Timer.StartTimer();
        }

        /// <summary>
        /// Queue up new words from Markov generation.
        /// </summary>
        private void QueueNewWords() {
            var splitWords =_markovChainGenerator.GetText(_generatedTextCount).Split(' ');
            foreach (var word in splitWords) {
                Queue.AddLast(BootStrapper.Container.ResolveKeyed<IWord>("Generated", new NamedParameter("word", word + " ")));
            }
        }
    }
}