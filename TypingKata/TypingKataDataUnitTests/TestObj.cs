using Newtonsoft.Json;

namespace TypingKataDataUnitTests {
    internal class TestObj {
        [JsonProperty]
        public string TestString { get; }

        public TestObj(string testString) {
            TestString = testString;
        }
    }
}