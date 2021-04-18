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

        [Test]
        public void ShouldReturnCorrectEventArgsWhenWordIncorrect() {
            _wordStackMock.Setup(x => x.Top).Returns(_mockWord.Object);
            var target = CreateTarget(_wordStackMock.Object, _messengerMock.Object, _markovChainGeneratorMock.Object);
            target.GeneratedWords.AddFirst(new LinkedListNode<IWord>(new GeneratedWord("test1 ")));
            target.GeneratedWords.AddLast(new LinkedListNode<IWord>(new GeneratedWord("test2 ")));
            var res = target.CompareAndCommitWords();
            _wordStackMock.Verify(x => x.Top);
            Assert.IsFalse(res.IsCorrect);
        }

        [Test]
        public void ShouldReturnNullWhenGeneratedWordsIsNull() {
            _wordStackMock.Setup(x => x.Top).Returns(_mockWord.Object);
            var target = CreateTarget(_wordStackMock.Object, _messengerMock.Object, _markovChainGeneratorMock.Object);
            var res = target.CompareAndCommitWords();
            _wordStackMock.Verify(x => x.Top);
            Assert.IsNull(res);
        }

        [Test]
        public void ShouldRemoveTopCharacterFromUserWordsOnBackspace() {
            var descriptor = new CharacterDescriptor("r", CharacterStatus.Incorrect);
            _mockWord.Setup(x => x[It.IsAny<int>()]).Returns(descriptor);
            _wordStackMock.Setup(x => x.Top).Returns(_mockWord.Object);
            _wordStackMock.Setup(x => x.Top.CharCount).Returns(4);
            var target = CreateTarget(_wordStackMock.Object, _messengerMock.Object, _markovChainGeneratorMock.Object);
            var res = target.HandleBackspace();

            _wordStackMock.Verify(x => x.Top);
            _wordStackMock.Verify(x => x.Top.CharCount);
            Assert.AreEqual(CharacterStatus.Incorrect, res);
        }

        [Test]
        public void ShouldResetCalculator() {
            _wordStackMock.Setup(x => x.ClearStack());
            var target = CreateTarget(_wordStackMock.Object, _messengerMock.Object, _markovChainGeneratorMock.Object);
            target.ResetCalculator();
            _wordStackMock.Verify(x => x.ClearStack());
        }

        private TypingSpeedCalculator CreateTarget(IWordStack userWords, ITinyMessengerHub tinyMessengerHub,
            IMarkovChainGenerator markovChainGenerator) {
            return new TypingSpeedCalculator(userWords, tinyMessengerHub, markovChainGenerator);
        }

    }
}