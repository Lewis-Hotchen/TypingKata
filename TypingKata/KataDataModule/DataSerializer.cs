﻿using System.IO;
using Newtonsoft.Json;

namespace KataDataModule {
    public class DataSerializer : IDataSerializer {

        public void SerializeObject(object data, string path) {

            var serializer = new JsonSerializer();

            using (var sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw)) {
                serializer.Serialize(writer, data);
            }
        }

        public T DeserializeObject<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}