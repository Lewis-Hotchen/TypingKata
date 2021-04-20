using System;
using System.Collections.Generic;
using KataIocModule;
using KataSpeedProfilerModule;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;
using Moq;
using NUnit.Framework;

namespace TypingKataSpeedProfilerUnitTests {

    [TestFixture]
    public class TypingProfilerUnitTest {
        private Mock<IMarkovChainGenerator> _mockMarkovGenerator;
        private Mock<IWordStack> _wordStackMock;
        private Mock<ICursor> _cursorMock;
        private Mock<ITypingTimer> _timerMock;
        private List<Mock<IWord>> _mockStackWords;
        private Mock<ITinyMessengerHub> _mockTinyMessengerHub;
        private Mock<ITypingSpeedCalculator> _calculatorMock;

        [SetUp]
        public void Setup() {
            _mockMarkovGenerator = new Mock<IMarkovChainGenerator>();

            _mockMarkovGenerator.Setup(x => x.GetText(200)).Returns("This is a test");
            _mockTinyMessengerHub = new Mock<ITinyMessengerHub>();
            var mockWord1 = new Mock<IWord>();
            mockWord1.Setup(x => x[It.IsAny<int>()]).Returns(new CharacterDescriptor("t", CharacterStatus.Correct));
            mockWord1.Setup(x => x.Chars).Returns(new List<CharacterDescriptor> {
                new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("e", CharacterStatus.Unmodified),
                new CharacterDescriptor("s", CharacterStatus.Unmodified),
                new CharacterDescriptor("t", CharacterStatus.Unmodified)
            });
            mockWord1.Setup(x => x.CharCount).Returns(4);
            var mockWord2 = new Mock<IWord>();
            mockWord2.Setup(x => x.Chars).Returns(new List<CharacterDescriptor> {
                new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("e", CharacterStatus.Unmodified),
                new CharacterDescriptor("s", CharacterStatus.Unmodified),
                new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("2", CharacterStatus.Unmodified)
            });

            mockWord2.Setup(x => x.CharCount).Returns(5);
            var mockWord3 = new Mock<IWord>();
            mockWord3.Setup(x => x.Chars).Returns(new List<CharacterDescriptor> {
                new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("e", CharacterStatus.Unmodified),
                new CharacterDescriptor("s", CharacterStatus.Unmodified),
                new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("3", CharacterStatus.Unmodified)
            });
            mockWord3.Setup(x => x.CharCount).Returns(5);

            _calculatorMock = new Mock<ITypingSpeedCalculator>();
            _wordStackMock = new Mock<IWordStack>();
            _cursorMock = new Mock<ICursor>();
            _timerMock = new Mock<ITypingTimer>();
            _timerMock.Setup(x => x.Time).Returns(new TimeSpan(0, 0, 1, 60, 1000 * 60));
            _mockStackWords = new List<Mock<IWord>>(new[] { mockWord1, mockWord2, mockWord3 });
            var mockStackWordsObj = new IWord[_mockStackWords.Count];
            for (var i = 0; i < mockStackWordsObj.Length; i++) {
                mockStackWordsObj[i] = _mockStackWords[i].Object;
            }

            _wordStackMock.Setup(x => x.GetWordsAsArray()).Returns(mockStackWordsObj);
            _calculatorMock.Setup(x => x.UserWords).Returns(_wordStackMock.Object);
            _calculatorMock.Setup(x => x.GenerateWords(It.IsAny<int>())).Returns(new[] {"test "});
            _calculatorMock.Setup(x => x.GeneratedWords).Returns(new LinkedList<IWord>(mockStackWordsObj));
        }

        [Test]
        public void ShouldResetOnCompletionOfTimer() {
            _calculatorMock.Setup(x => x.ResetCalculator());
            _calculatorMock.Setup(x => x.CalculateWpm(It.IsAny<int>()));
            _cursorMock.Setup(x => x.ResetCursor());
            _mockTinyMessengerHub.Setup(x => x.Publish(It.IsAny<TestCompleteMessage>()));
            var target = CreateTarget(_cursorMock.Object, _calculatorMock.Object, _timerMock.Object,
                _mockTinyMessengerHub.Object);
            target.Start();
            
            _timerMock.Raise(x => x.TimeComplete += null, null, null);
            _calculatorMock.Verify(x => x.ResetCalculator());
            _calculatorMock.Verify(x => x.CalculateWpm(It.IsAny<int>()));
            _cursorMock.Verify(x => x.ResetCursor());
            _mockTinyMessengerHub.Verify(x => x.Publish(It.IsAny<TestCompleteMessage>()));
        }

        [TestCase('t')]
        [TestCase('e')]
        [TestCase('s')]
        [TestCase('t')]
        public void ShouldCommitLetterCorrectly(char input) {
            var isCorrect = false;
            var correctChar = ' ';
            _cursorMock.Setup(x => x.CurrentWord).Returns(new GeneratedWord(new string(new []{input})));
            _cursorMock.Setup(x => x.CharPos).Returns(0);
            _cursorMock.Setup(x => x.NextChar(1));
            _wordStackMock.Setup(x => x.Top).Returns(_mockStackWords[0].Object);
            var target = CreateTarget(_cursorMock.Object, _calculatorMock.Object, _timerMock.Object,
                _mockTinyMessengerHub.Object);
            target.KeyComplete += (sender, args) => {
                isCorrect = args.IsCorrect;
                correctChar = args.InputKey;
            };
            target.Start();
            target.CharacterInput(input);
            _cursorMock.Verify(x => x.CurrentWord);
            _cursorMock.Verify(x => x.NextChar(1));
            Assert.AreEqual(input, correctChar);
            Assert.AreEqual(true, isCorrect);
        }

        [Test]
        public void ShouldCommitLetterCorrectlyWhenBackspace() {
            var isCorrect = false;
            var correctChar = '\b';
            _cursorMock.Setup(x => x.CurrentWord).Returns(_mockStackWords[0].Object);
            _wordStackMock.Setup(x => x.Top).Returns(_mockStackWords[0].Object);
            var target = CreateTarget(_cursorMock.Object, _calculatorMock.Object, _timerMock.Object,
                _mockTinyMessengerHub.Object);
            target.KeyComplete += (sender, args) => {
                isCorrect = args.IsCorrect;
                correctChar = args.InputKey;
            };
            target.Start();

            target.CharacterInput('t');
            target.CharacterInput('\b');

            Assert.AreEqual('\b', correctChar);
            Assert.AreEqual(true, isCorrect);
        }

        [Test]
        public void ShouldReturnWhenBackspacingWhenCursorAt0() {
            _cursorMock.Setup(x => x.WordPos).Returns(0);
            _cursorMock.Setup(x => x.CharPos).Returns(0);
            var target = CreateTarget(_cursorMock.Object, _calculatorMock.Object, _timerMock.Object,
                _mockTinyMessengerHub.Object);
            target.Start();
            target.CharacterInput('\b');

            _cursorMock.Verify(x => x.WordPos);
            _cursorMock.Verify(x => x.CharPos);
        }

        private TypingProfiler CreateTarget(ICursor cursor, ITypingSpeedCalculator calculator, ITypingTimer timer, ITinyMessengerHub messengerHub) {
            return new TypingProfiler(cursor, calculator, timer, messengerHub);
        }
    }
}
