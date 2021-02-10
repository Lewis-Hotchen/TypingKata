using System.Collections;
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
            _mockWord.Setup(w => w.Chars).Returns(new[] {'h', 'e', 'l', 'l', 'o'});
            _mockWord.Setup(w => w.ToString()).Returns("hello");

            _mockWord2 = new Mock<IWord>();
            _mockWord2.Setup(w => w.Chars).Returns(new[] {'b', 'y', 'e'});
            _mockWord2.Setup(w => w.ToString()).Returns("hello");
        }

        [Test]
        public void ShouldConvertStackToArray() {
            var expected = new IWord[] {new GeneratedWord("hello"), new GeneratedWord("") };
            var target = CreateTarget();
            
            target.Push(_mockWord.Object);
            var result = target.GetWordsAsArray();
            Assert.AreEqual(expected.Length, result.Length);
            Assert.AreEqual(expected[0].Chars, result[0].Chars);
        }

        [Test]
        public void ShouldAddNewWordToStackAndBeNewTop() {
            var target = CreateTarget();
            var expected = new GeneratedWord("hello");
            var expected2 = new GeneratedWord("bye");

            target.Push(_mockWord.Object);
            Assert.AreEqual(2, target.Count);
            Assert.AreEqual(expected.Chars, target.Top.Chars);

            target.Push(_mockWord2.Object);
            Assert.AreEqual(3, target.Count);
            Assert.AreEqual(expected2.Chars, target.Top.Chars);
        }

        public WordStack CreateTarget() {
            return new WordStack();
        }

    }

}
