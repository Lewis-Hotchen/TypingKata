using System;
using System.Collections.Generic;
using System.Linq;
using KataSpeedProfilerModule;
using KataSpeedProfilerModule.Interfaces;
using Moq;
using NUnit.Framework;

namespace SpeedProfilerUnitTests {

    [TestFixture]
    public class TypingProfilerUnitTest {
        private Mock<IWordStack> _wordStackMock;
        private Mock<IWordQueue> _wordQueueMock;
        private Mock<ICursor> _cursorMock;
        private Mock<ITypingTimer> _timerMock;

        private List<Mock<IWord>> _mockQueueWords;
        private List<Mock<IWord>> _mockStackWords;

        [SetUp]
        public void Setup() {

            var mockWord1 = new Mock<IWord>();
            mockWord1.Setup(x => x.Chars).Returns(new List<char> {'t', 'e', 's', 't'});
            mockWord1.Setup(x => x.CharCount).Returns(4);
            var mockWord2 = new Mock<IWord>();
            mockWord2.Setup(x => x.Chars).Returns(new List<char> { 't', 'e', 's', 't', '2' });
            mockWord2.Setup(x => x.CharCount).Returns(5);
            var mockWord3 = new Mock<IWord>();
            mockWord3.Setup(x => x.Chars).Returns(new List<char> { 't', 'e', 's', 't', '3' });
            mockWord3.Setup(x => x.CharCount).Returns(5);

            _wordStackMock = new Mock<IWordStack>();
            _wordQueueMock = new Mock<IWordQueue>();
            _cursorMock = new Mock<ICursor>();
            _timerMock = new Mock<ITypingTimer>();
            _timerMock.Setup(x => x.Time).Returns(new TimeSpan(0, 0, 1, 60, 1000 * 60));
            _mockQueueWords = new List<Mock<IWord>>(new[] { mockWord1, mockWord2, mockWord3 });
            _mockStackWords = new List<Mock<IWord>>(new[] { mockWord1, mockWord2, mockWord3 });
            var mockStackWordsObj = new IWord[_mockStackWords.Count];
            for (var i = 0; i < mockStackWordsObj.Length; i++) {
                mockStackWordsObj[i] = _mockStackWords[i].Object;
            }

            _wordStackMock.Setup(x => x.GetWordsAsArray()).Returns(mockStackWordsObj);
        }

        [Test]
        public void ShouldResetOnCompletionOfTimer() {
            var target = CreateTarget(_wordStackMock.Object, _wordQueueMock.Object, _cursorMock.Object, _timerMock.Object);
            var testCompleteActual = false;
            target.TestCompleteEvent += (sender, args) => testCompleteActual = true;
            _cursorMock.Setup(x => x.ResetCursor());
            _wordQueueMock.Setup(x => x.ClearQueue());
            _timerMock.Raise(x => x.TimeComplete += null, null, new EventArgs());

            _cursorMock.VerifyAll();
            _wordQueueMock.VerifyAll();
            Assert.AreEqual(true, testCompleteActual);
        }

        [Test]
        [Ignore("Incomplete test.")]
        public void ShouldReturnCorrectWpm() {
            var target = CreateTarget(_wordStackMock.Object, _wordQueueMock.Object, _cursorMock.Object, _timerMock.Object);
            _timerMock.SetupGet(x => x.Time).Returns(new TimeSpan(0, 1, 0));
            _timerMock.Raise(x => x.TimeComplete += null, null, new EventArgs());

            _wordStackMock.VerifyAll();
            _timerMock.Verify(x => x.Time, Times.Exactly(2));
        }


        [TestCase(1, 300)]
        [TestCase(3, 500)]
        [TestCase(2, 100)]
        [Ignore("Unknown error, needs fixed or deleted.")]
        public void ShouldPopulateQueueUsingMarkovTextGenerator(int keySize, int outputSize) {
            var target = CreateTarget(_wordStackMock.Object, _wordQueueMock.Object, _cursorMock.Object, _timerMock.Object);
            _wordQueueMock.Setup(x => x.Enqueue(It.IsAny<IWord>()));
            target.Start(keySize, outputSize);
            

            _wordQueueMock.Verify(x => x.Enqueue(It.IsAny<IWord>()), Times.Exactly(outputSize));
        }

        private TypingProfiler CreateTarget(IWordStack  wordStack, IWordQueue queue, ICursor cursor, ITypingTimer timer) {
            return new TypingProfiler(cursor, wordStack, queue, timer);
        }

    }
}
