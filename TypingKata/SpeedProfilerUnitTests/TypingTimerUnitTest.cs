using KataSpeedProfilerModule;
using NUnit.Framework;
using System.Threading;

namespace SpeedProfilerUnitTests {

    [TestFixture]
    public class TypingTimerUnitTest {

        [SetUp]
        public void Setup() {

        }

        [TestCase(60)]
        [TestCase(120)]
        public void ShouldInstantiateWithCorrectTime(double time) {
            var target = CreateTarget(time);

            Assert.AreEqual(target.Time.TotalSeconds, time);
        }

        [Test]
        public void ShouldRaiseTimeElapsedOnTimerElapsed() {
            var target = CreateTarget(1);
            var called = false;
            target.TimeComplete += (sender, args) => called = true;

            target.StartTimer();
            Thread.Sleep(1000);
            Assert.IsTrue(called);


        }

        private TypingTimer CreateTarget(double time) {
            return new TypingTimer(time);
        }
    }
}
