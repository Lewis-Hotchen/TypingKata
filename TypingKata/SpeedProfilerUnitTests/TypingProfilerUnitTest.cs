using System;
using System.Collections.Generic;
using KataIocModule;
using KataSpeedProfilerModule;
using KataSpeedProfilerModule.Interfaces;
using Moq;
using NUnit.Framework;

namespace SpeedProfilerUnitTests {

    [TestFixture]
    public class TypingProfilerUnitTest {
        private Mock<IMarkovChainGenerator> _mockMarkovGenerator;
        private Mock<IWordStack> _wordStackMock;
        private Mock<ICursor> _cursorMock;
        private Mock<ITypingTimer> _timerMock;
        private List<Mock<IWord>> _mockStackWords;
        private Mock<ITinyMessengerHub> _mockTinyMessengerHub;

        [SetUp]
        public void Setup() {
            _mockMarkovGenerator = new Mock<IMarkovChainGenerator>();

            _mockMarkovGenerator.Setup(x => x.GetText(200)).Returns("This is a test");
            _mockTinyMessengerHub = new Mock<ITinyMessengerHub>();
            var mockWord1 = new Mock<IWord>();
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
        }

        [Test]
        public void ShouldResetOnCompletionOfTimer() {
            var target = CreateTarget(_wordStackMock.Object, _cursorMock.Object, _timerMock.Object, _mockMarkovGenerator.Object, _mockTinyMessengerHub.Object);
            _cursorMock.Setup(x => x.ResetCursor());
            _timerMock.Raise(x => x.TimeComplete += null, null, new EventArgs());

            _cursorMock.VerifyAll();
        }

        [Test]
        [Ignore("Strange library bug, unsolved as of now.")]
        public void ShouldReturnCorrectWpm() {
            var target = CreateTarget(_wordStackMock.Object, _cursorMock.Object, _timerMock.Object, _mockMarkovGenerator.Object, _mockTinyMessengerHub.Object);
            _timerMock.SetupGet(x => x.Time).Returns(new TimeSpan(0, 1, 0));
            _timerMock.Raise(x => x.TimeComplete += null, null, new EventArgs());
            target.Start();
            SimulateTyping(target);

            _wordStackMock.VerifyAll();
            _timerMock.Verify(x => x.Time, Times.Exactly(2));
        }

        private TypingProfiler CreateTarget(IWordStack  wordStack, ICursor cursor, ITypingTimer timer, IMarkovChainGenerator markovChainGenerator, ITinyMessengerHub messengerHub) {
            return new TypingProfiler(cursor, wordStack, timer, markovChainGenerator, messengerHub);
        }

        private void SimulateTyping(TypingProfiler target) {
            foreach (var characterDescriptor in _mockStackWords[0].Object.Chars) {
                target.CharacterInput(characterDescriptor.CurrentCharacter.ToCharArray()[0]);
            }
        }
    }
}
