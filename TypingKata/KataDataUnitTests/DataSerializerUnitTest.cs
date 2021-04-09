using System;
using KataDataModule;
using Newtonsoft.Json;
using NUnit.Framework;

namespace KataDataUnitTests {

    public class DataSerializerUnitTest {

        [SetUp]
        public void Setup() {
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenPathEmpty() {
            var target = CreateTarget();

            Assert.Throws<ArgumentException>(() => target.SerializeObject(new object(), ""));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenObjectNull() {
            var target = CreateTarget();

            Assert.Throws<ArgumentException>(() => target.SerializeObject(null, ""));
        }

        [Test]
        public void ShouldConvertJsonObject() {
            var target = CreateTarget();

            var res = target.DeserializeObject<TestObj>("{\"TestString\" : \"test\"}");
            Assert.NotNull(res);
            Assert.AreEqual("test", res.TestString);
        }

        public DataSerializer CreateTarget() {
            return new DataSerializer();
        }
    }

    internal class TestObj {
        [JsonProperty]
        public string TestString { get; }

        public TestObj(string testString) {
            TestString = testString;
        }
    }
}