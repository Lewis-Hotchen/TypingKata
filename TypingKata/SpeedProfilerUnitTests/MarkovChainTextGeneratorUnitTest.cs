using KataSpeedProfilerModule;
using NUnit.Framework;

namespace SpeedProfilerUnitTests {

    [TestFixture]
    public class MarkovChainTextGeneratorUnitTest {
        /// <summary>
        /// Need to use the text file from the original project as it using executing assembly so a test
        /// file cannot be added here.
        /// </summary>
        private const string path = "KataSpeedProfilerModule.Resources.words.txt";

        [SetUp]
        public void SetUp() {
        }

        [Test]
        public void ShouldReturnWordsFromTextFile() {
            var target = CreateTarget();
            var res = target.GetText(20);
            Assert.IsNotEmpty(res);
        }

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        public void ShouldReturnWordsWithCorrectAmountOfWords(int noOfWords) {
            var target = CreateTarget();
            var res = target.GetText(noOfWords);

            var splitText = res.Split(' ');

            Assert.AreEqual(noOfWords, splitText.Length);
        }

        public MarkovChainGenerator CreateTarget() {
            return new MarkovChainGenerator(path);
        }

    }
}
