using KataSpeedProfilerModule;
using KataSpeedProfilerModule.Interfaces;
using Moq;
using NUnit.Framework;

namespace SpeedProfilerUnitTests {

    [TestFixture]
    public class WordQueueUnitTest {
        private Mock<IWord> _mockWord;

        [SetUp]
        public void Setup() {
            _mockWord = new Mock<IWord>();
            _mockWord.Setup(w => w.Chars).Returns(new[] {'t', 'e', 's', 't'});
        }

        [Test]
        public void ShouldQueueNewWord() {
            var target = CreateTarget();
            var expected = new[] {'t', 'e', 's', 't'};
            target.Enqueue(_mockWord.Object);

            Assert.AreEqual(expected, target.Top.Chars);
        }

        [Test]
        public void ShouldDequeueWord() {
            var target = CreateTarget();
            target.Enqueue(_mockWord.Object);
            target.Dequeue();

            Assert.AreEqual(0, target.Count);
        }

        public WordQueue CreateTarget() {
            return new WordQueue();
        }
    }
}