using Newtonsoft.Json;

namespace KataDataModule.JsonObjects
{
    [JsonObject]
    public class SettingJsonObject
    {

        [JsonProperty]
        public object Data { get; set; }
        [JsonProperty]
        public string Name { get; set; }

        public SettingJsonObject(object data, string name) {
            Data = data;
            Name = name;
        }
    }
}
