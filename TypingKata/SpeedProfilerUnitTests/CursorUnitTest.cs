﻿using System.Collections.Generic;
using KataSpeedProfilerModule;
using KataSpeedProfilerModule.Interfaces;
using Moq;
using NUnit.Framework;

namespace SpeedProfilerUnitTests {

    [TestFixture]
    public class CursorUnitTest {
        private Mock<IWord> _mockWord;

        [SetUp]
        public void Setup() {
            _mockWord = new Mock<IWord>();
            _mockWord.Setup(x => x.Chars).Returns(new List<CharacterDescriptor> {
                new CharacterDescriptor("t", CharacterStatus.Unmodified),
                new CharacterDescriptor("e", CharacterStatus.Unmodified),
                new CharacterDescriptor("s", CharacterStatus.Unmodified),
                new CharacterDescriptor("t", CharacterStatus.Unmodified)
            });
            _mockWord.Setup(x => x.CharCount).Returns(4);

        }

        [Test]
        public void ShouldInitialiseWithCorrectWord() {
            var target = CreateTargetWithWord(_mockWord.Object);
            Assert.AreEqual(_mockWord.Object.Chars, target.CurrentWord.Chars);
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        public void ShouldIncrementCharacterCorrectly(int expected, int increment) {
            var target = CreateTargetWithWord(_mockWord.Object);
            target.NextChar(increment);
            Assert.AreEqual(expected, target.CharPos);
        }

        [Test]
        public void ShouldNotDecrementWhenCharPosIsZero() {
            var target = CreateTargetWithWord(_mockWord.Object);
            target.NextChar(-1);
            Assert.AreEqual(0, target.CharPos);
        }

        [TestCase(0, -1, 1)]
        [TestCase(0, -2, 2)]
        [TestCase(0, -3, 3)]
        public void ShouldDecrementCharacterCorrectly(int expected, int decrement, int increment) {
            var target = CreateTargetWithWord(_mockWord.Object);
            target.NextChar(increment);
            target.NextChar(decrement);

            Assert.AreEqual(expected, target.CharPos);
        }

        private Cursor CreateTarget() {
            return new Cursor();
        }

        private Cursor CreateTargetWithWord(IWord word) {
            var cursor = new Cursor();
            cursor.NextWord(1, word);
            return cursor;
        }
    }
}