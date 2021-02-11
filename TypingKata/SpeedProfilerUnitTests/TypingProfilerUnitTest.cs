using System;
using System.Collections.Generic;
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
            var mockWord2 = new Mock<IWord>();
            mockWord1.Setup(x => x.Chars).Returns(new List<char> { 't', 'e', 's', 't', '2' });
            var mockWord3 = new Mock<IWord>();
            mockWord1.Setup(x => x.Chars).Returns(new List<char> { 't', 'e', 's', 't', '3' });

            _wordStackMock = new Mock<IWordStack>();
            _wordQueueMock = new Mock<IWordQueue>();
            _cursorMock = new Mock<ICursor>();
            _timerMock = new Mock<ITypingTimer>();
            _mockQueueWords = new List<Mock<IWord>>(new[] { mockWord1, mockWord2, mockWord3 });
            _mockStackWords = new List<Mock<IWord>>(new[] { mockWord1, mockWord2, mockWord3 });
        }

        [Test]
        public void ShouldResetOnCompletionOfTimer() {
            var target = CreateTarget(_wordStackMock.Object, _wordQueueMock.Object, _cursorMock.Object, _timerMock.Object);
            _cursorMock.Setup(x => x.ResetCursor());
            _wordQueueMock.Setup(x => x.ClearQueue());
            _timerMock.Raise(x => x.TimeComplete += null, null, new EventArgs());

            _cursorMock.VerifyAll();
            _wordQueueMock.VerifyAll();
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
