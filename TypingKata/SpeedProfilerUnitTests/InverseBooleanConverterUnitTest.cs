using System.Globalization;
using KataSpeedProfilerModule;
using NUnit.Framework;

namespace TypingKataSpeedProfilerUnitTests {

    [TestFixture]
    public class InverseBooleanConverterUnitTest {

        [SetUp]
        public void Setup() { }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldInvertBooleanCorrectly(bool value) {
            var target = CreateTarget();

            var res = target.Convert(value, typeof(bool),
                null, CultureInfo.DefaultThreadCurrentCulture);

            Assert.AreEqual(!value, res);
        }


        public InverseBooleanConverter CreateTarget() {
            return new InverseBooleanConverter();
        }
    }
}
