using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using KataIocModule;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {
    public class TypingSpeedCalculator : ITypingSpeedCalculator {
        
        private readonly ITinyMessengerHub _messengerHub;

        //Choose 200 as an upper bound as to not generate more text than needed. Highly unlikely that anyone could type more
        //than 200wpm.
        private int _generatedTextCount;
        private readonly IMarkovChainGenerator _markovChainGenerator;

        /// <summary>
        /// The words written by the user.
        /// </summary>
        public IWordStack UserWords { get; }

        /// <summary>
        /// The incorrect words written.
        /// </summary>
        public List<(IWord, IWord)> ErrorWords { get; }
        
        /// <summary>
        /// Collection of the generated words to type.
        /// </summary>
        public LinkedList<IWord> GeneratedWords { get; }

        /// <summary>
        /// List of words removed from the generated words once typed.
        /// </summary>
        public LinkedList<IWord> RemovedWords { get; }

        /// <summary>
        /// Instantiate Speed calculator.
        /// </summary>
        /// <param name="userWords"></param>
        /// <param name="messengerHub"></param>
        /// <param name="markovChainGenerator"></param>
        public TypingSpeedCalculator(IWordStack userWords, ITinyMessengerHub messengerHub, IMarkovChainGenerator markovChainGenerator) {
            UserWords = userWords;
            _messengerHub = messengerHub;
            _markovChainGenerator = markovChainGenerator;
            ErrorWords = new List<(IWord, IWord)>();
            GeneratedWords = new LinkedList<IWord>();
            RemovedWords = new LinkedList<IWord>();
        }

        /// <summary>
        /// Calculate the WPM.
        /// </summary>
        /// <param name="time">The time of the test.</param>
        /// <returns>WPM results message.</returns>
        public TestCompleteMessage CalculateWpm(int time) {
            var words = UserWords.GetWordsAsArray();
            var noOfCharsTyped = words.Sum(x => x.CharCount);
            var grossWpm = Convert.ToInt32((noOfCharsTyped / 5) / time);
            var wpm = grossWpm - (ErrorWords.Count / time);
            var errorRate = ((double)UserWords.Count / 100) * ErrorWords.Count;

            return new TestCompleteMessage(this, new TestCompleteEventArgs(errorRate, ErrorWords, wpm));
        }

        /// <summary>
        /// Compare the current written words and compare them.
        /// </summary>
        /// <returns>Event args to be sent for word changed.</returns>
        public WordChangedEventArgs CompareAndCommitWords() {
            var userWord = UserWords.Top;
            var generatedWord = GeneratedWords.First;
            var isCorrect = false;

            if (GeneratedWords.First == null || generatedWord == null)
                return null;

            var userWordString = UserWords.Top.ToString() + ' ';
            var generatedWordString = GeneratedWords.First.Value.ToString();

            if (string.Equals(userWordString, generatedWordString, StringComparison.CurrentCultureIgnoreCase)) {
                isCorrect = true;

            }

            (IWord, IWord) last = (null, null);

            if (!isCorrect) {
                ErrorWords.Add((userWord, generatedWord.Value));

                if (ErrorWords.Any()) {
                    last = ErrorWords.Last();
                }
                
            }
           
            RemovedWords.AddLast(new LinkedListNode<IWord>(generatedWord.Value));
            GeneratedWords.RemoveFirst();
            UserWords.Push(new UserDefinedWord());

            return isCorrect ? 
                  new WordChangedEventArgs(generatedWord.Value, GeneratedWords.First.Value, true)
                : new WordChangedEventArgs(last.Item1, last.Item2, false);
        }

        /// <summary>
        /// Calculate appropriate collections on a backspace.
        /// </summary>
        /// <returns></returns>
        public CharacterStatus HandleBackspace() {
            var status = UserWords.Top[UserWords.Top.CharCount - 1].Status;
            UserWords.Top.Chars.RemoveAt(UserWords.Top.CharCount - 1);
            return status;
        }

        /// <summary>
        /// Calculate appropriate collections on a backspace when the word changes.
        /// </summary>
        public void HandleChangeWordOnBackspace() {
            GeneratedWords.AddFirst(new LinkedListNode<IWord>(RemovedWords.Last.Value));
            RemovedWords.RemoveLast();
            UserWords.Pop();
        }

        /// <summary>
        /// Reset the calculator.
        /// </summary>
        public void ResetCalculator() {
            GeneratedWords.Clear();
            RemovedWords.Clear();
            UserWords.ClearStack();
            ErrorWords.Clear();
        }

        /// <summary>
        /// Generate words for typing test.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        public IEnumerable<string> GenerateWords(int minutes) {
            _generatedTextCount = 200 * minutes;
            var generatedWords = _markovChainGenerator.GetText(_generatedTextCount).Split(' ');

            foreach (var word in generatedWords) {
                GeneratedWords.AddLast(
                    BootStrapper.Container.ResolveKeyed<IWord>("Generated", new NamedParameter("word", word + " "))
                    );
            }

            foreach (var word in GeneratedWords) {
                yield return string.Join("", word.Chars.Select(x => x.CurrentCharacter));
            }
        }
    }
}