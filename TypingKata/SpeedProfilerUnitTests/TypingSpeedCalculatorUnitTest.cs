using System.Collections.Generic;
using KataIocModule;
using KataSpeedProfilerModule;
using KataSpeedProfilerModule.Interfaces;
using Moq;
using NUnit.Framework;

namespace TypingKataSpeedProfilerUnitTests {
    public class TypingSpeedCalculatorUnitTest {

        private Mock<ITinyMessengerHub> _messengerMock;
        private Mock<IWordStack> _wordStackMock;
        private Mock<IMarkovChainGenerator> _markovChainGeneratorMock;
        private Mock<IWord> _mockWord;

        [SetUp]
        public void Setup() {

            _messengerMock = new Mock<ITinyMessengerHub>();
            _wordStackMock = new Mock<IWordStack>();
            _markovChainGeneratorMock = new Mock<IMarkovChainGenerator>();
            _mockWord = new Mock<IWord>();
            _mockWord.Setup(x => x.Chars).Returns(new List<CharacterDescriptor> {
                new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("e", CharacterStatus.Unmodified),
                new CharacterDescriptor("s", CharacterStatus.Unmodified),
                new CharacterDescriptor("t", CharacterStatus.Unmodified)
            });

            _mockWord.Setup(x => x.ToString()).Returns("test");
        }

        [Test]
        public void ShouldReturnCorrectEventArgsWhenWordCorrect() {
            _wordStackMock.Setup(x => x.Top).Returns(_mockWord.Object);

            var target = CreateTarget(_wordStackMock.Object, _messengerMock.Object, _markovChainGeneratorMock.Object);
            target.GeneratedWords.AddFirst(new LinkedListNode<IWord>(new GeneratedWord("test ")));
            target.GeneratedWords.AddLast(new LinkedListNode<IWord>(new GeneratedWord("test2 ")));
            var res = target.CompareAndCommitWords();
            _wordStackMock.Verify(x => x.Top);
            Assert.IsTrue(res.IsCorrect);
        }

        private TypingSpeedCalculator CreateTarget(IWordStack userWords, ITinyMessengerHub tinyMessengerHub,
            IMarkovChainGenerator markovChainGenerator) {
            return new TypingSpeedCalculator(userWords, tinyMessengerHub, markovChainGenerator);
        }

    }
}
