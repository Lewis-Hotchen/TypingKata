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
        private readonly ITinyMessengerHub _messengerHub;

        //Choose 200 as an upper bound as to not generate more text than needed. Highly unlikely that anyone could type more
        //than 200wpm.
        private int _generatedTextCount;
        private string[] _generatedWords;
        public ICursor Cursor { get; }
        public IWordStack UserWords { get; }
        public List<(IWord, IWord)> ErrorWords { get; }
        public ITypingTimer Timer { get; }
        public LinkedList<IWord> GeneratedWords { get; }
        public LinkedList<IWord> RemovedWords { get; }
        public event EventHandler<KeyInputEventHandlerArgs> KeyComplete;
        public event EventHandler<WordChangedEventArgs> NextWordEvent;
        public event EventHandler<BackspaceCompleteEvent> BackspaceCompleteEvent;

        /// <summary>
        /// Instantiate new TypingProfiler.
        /// </summary>
        /// <param name="cursor">The Cursor.</param>
        /// <param name="userWords">The user words.</param>
        /// <param name="timer">The timer.</param>
        /// <param name="markovChainGenerator"></param>
        /// <param name="messengerHub"></param>
        public TypingProfiler(ICursor cursor,  IWordStack userWords, ITypingTimer timer, IMarkovChainGenerator markovChainGenerator, ITinyMessengerHub messengerHub) {
            _markovChainGenerator = markovChainGenerator;
            _messengerHub = messengerHub;
            Cursor = cursor;
            UserWords = userWords;
            ErrorWords = new List<(IWord, IWord)>();
            GeneratedWords = new LinkedList<IWord>();
            RemovedWords = new LinkedList<IWord>();
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
            GeneratedWords.Clear();
            RemovedWords.Clear();

            //Calculate wpm
            CalculateWpm();
        }

        /// <summary>
        /// Calculate the wpm.
        /// </summary>
        private void CalculateWpm() {
            var words = UserWords.GetWordsAsArray();
            var noOfCharsTyped = words.Sum(x => x.CharCount);
            var grossWpm = Convert.ToInt32((noOfCharsTyped / 5) / Timer.Time.TotalMinutes);
            var wpm = (int) (grossWpm - (ErrorWords.Count / Timer.Time.TotalMinutes));
            var errorRate = ((double) UserWords.Count / 100) * ErrorWords.Count;

            _messengerHub.Publish(new TestCompleteMessage(this, new TestCompleteEventArgs(errorRate, ErrorWords, wpm)));
        }

        /// <summary>
        /// Handle backspace input.
        /// </summary>
        private void HandleBackspace() {
            if (Cursor.WordPos == 0 && Cursor.CharPos == 0) return;

            var userWord = UserWords.Top;

            if (userWord.CharCount == 0) {
                //go back a word
                GeneratedWords.AddFirst(new LinkedListNode<IWord>(RemovedWords.Last.Value));
                RemovedWords.RemoveLast();
                UserWords.Pop();
                Cursor.NextWord(-1, GeneratedWords.First.Value);
                Cursor.NextChar(Cursor.CurrentWord.CharCount - 1);
                var c = Cursor.CurrentWord[Cursor.CharPos].CurrentCharacter[0];
                BackspaceCompleteEvent?.Invoke(this, new BackspaceCompleteEvent(' ', CharacterStatus.Correct));
            } else if (userWord.CharCount > 0) {
                //go back a character
                var status = UserWords.Top.Chars[UserWords.Top.CharCount - 1].Status;
                UserWords.Top.Chars.RemoveAt(UserWords.Top.CharCount - 1);
                Cursor.NextChar(-1);
                var c = Cursor.CurrentWord[Cursor.CharPos].CurrentCharacter[0];
                BackspaceCompleteEvent?.Invoke(this, new BackspaceCompleteEvent(c, status));
            }
        }

        /// <summary>
        /// Handle spacebar input.
        /// </summary>
        private void ConfirmSpace() {
            var userWord = UserWords.Top;
            var generatedWord = GeneratedWords.First;
            bool isWrong = false;
            var userWordString = UserWords.Top.ToString() + ' ';
            string generatedWordString = "";
            if (GeneratedWords.First == null || generatedWord == null)
                return;

            generatedWordString = GeneratedWords.First.Value.ToString();
            //Add word to Error words if they are not the same.
            if (!string.Equals(userWordString, generatedWordString, StringComparison.CurrentCultureIgnoreCase)) {
                ErrorWords.Add((userWord, generatedWord.Value));
                isWrong = true;
            }

            (IWord, IWord) last = (null, null);
            
            if (ErrorWords.Any()) {
                last = ErrorWords.Last();
            }

            RemovedWords.AddLast(new LinkedListNode<IWord>(generatedWord.Value));
            GeneratedWords.RemoveFirst();
            UserWords.Push(new UserDefinedWord());
            Cursor.NextWord(1, GeneratedWords.First.Value);

            NextWordEvent?.Invoke(this,
                isWrong
                    ? new WordChangedEventArgs(last.Item1, last.Item2, false)
                    : new WordChangedEventArgs(generatedWord.Value, GeneratedWords.First.Value, true));
        }

        /// <summary>
        /// Input a character.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void CharacterInput(char key) {
            if (key == '\b') {
                HandleBackspace();
                KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                return;
            }

            if (key == ' ') {
                if (UserWords.Top.CharCount != 0) {
                    ConfirmSpace();
                    KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                    return;
                }

                return;
            }

            //Change the case as Key.ToString() returns upper case.
            var casedChar = Cursor.CurrentWord[Cursor.CharPos].CurrentCharacter.ToUpper();
            if (casedChar[0] == char.ToUpper(key)) {
                UserWords.Top.Chars.Add(new CharacterDescriptor(new string(new []{key}), CharacterStatus.Correct));
                Cursor.NextChar(1);
                KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(true, key));
                return;
            }

            UserWords.Top.Chars.Add(new CharacterDescriptor(new string(new[] { key }), CharacterStatus.Incorrect));
            Cursor.NextChar(1);
            KeyComplete?.Invoke(this, new KeyInputEventHandlerArgs(false, key));
        }

        /// <summary>
        /// Start the test.
        /// </summary>
        public void Start() {
            
            //Determine number of words to generate.
            var minutes = Timer.Time.TotalMinutes;
            _generatedTextCount = (int) (200 * minutes);
            _generatedWords = _markovChainGenerator.GetText(_generatedTextCount).Split(' ');
            QueueNewWords(_generatedTextCount);
            Cursor.NextWord(0, GeneratedWords.First.Value);
            Timer.StartTimer();
        }

        /// <summary>
        /// GeneratedWords up new words.
        /// </summary>
        /// <param name="amount">The amount of words to queue.</param>
        private void QueueNewWords(int amount) {
            for (var i = 0; i < amount; i++) {
                GeneratedWords.AddLast(BootStrapper.Container.ResolveKeyed<IWord>("Generated", new NamedParameter("word", _generatedWords[i] + " ")));
            }
        }
    }
}