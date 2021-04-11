using Newtonsoft.Json;

namespace KataDataUnitTests {
    internal class TestObj {
        [JsonProperty]
        public string TestString { get; }

        public TestObj(string testString) {
            TestString = testString;
        }
    }
}