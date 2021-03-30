using System.Collections.Generic;
using KataSpeedProfilerModule;
using KataSpeedProfilerModule.Interfaces;
using Moq;
using NUnit.Framework;

namespace SpeedProfilerUnitTests {

    [TestFixture]
    public class WordStackUnitTest {
        private Mock<IWord> _mockWord;
        private Mock<IWord> _mockWord2;

        [SetUp]
        public void Setup() {

            _mockWord = new Mock<IWord>();
            _mockWord.Setup(w => w.Chars).Returns(new List<CharacterDescriptor> {new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("e", CharacterStatus.Unmodified),
                new CharacterDescriptor("s", CharacterStatus.Unmodified),
                new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("1", CharacterStatus.Unmodified)});
            _mockWord.Setup(w => w.ToString()).Returns("test1");

            _mockWord2 = new Mock<IWord>();
            _mockWord2.Setup(w => w.Chars).Returns(new List<CharacterDescriptor> {new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("e", CharacterStatus.Unmodified),
                new CharacterDescriptor("s", CharacterStatus.Unmodified),
                new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("2", CharacterStatus.Unmodified)});

            _mockWord2.Setup(w => w.ToString()).Returns("test2");
        }

        [Test]
        public void ShouldConvertStackToArray() {
            var expected = new IWord[] {new GeneratedWord("test1"), new GeneratedWord("") };
            var target = CreateTarget();
            
            target.Push(_mockWord.Object);
            var result = target.GetWordsAsArray();
            Assert.AreEqual(expected.Length, result.Length);
            Assert.AreEqual(expected[0].Chars.ToString(), result[0].Chars.ToString());
        }

        [Test]
        public void ShouldAddNewWordToStackAndBeNewTop() {
            var target = CreateTarget();
            var expected = new GeneratedWord("test1");
            var expected2 = new GeneratedWord("test2");

            target.Push(_mockWord.Object);
            Assert.AreEqual(2, target.Count);
            Assert.AreEqual(expected.Chars.ToString(), target.Top.Chars.ToString());

            target.Push(_mockWord2.Object);
            Assert.AreEqual(3, target.Count);
            Assert.AreEqual(expected2.Chars.ToString(), target.Top.Chars.ToString());
        }

        public WordStack CreateTarget() {
            return new WordStack();
        }

    }
}