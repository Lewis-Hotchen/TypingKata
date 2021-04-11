using System;
using System.IO;
using KataDataModule.Interfaces;
using Newtonsoft.Json;

namespace KataDataModule {

    /// <summary>
    /// Class to handle serializing and deserializing of JSON.
    /// </summary>
    public class DataSerializer : IDataSerializer {


        /// <summary>
        /// Serialize an object.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="path">Json File path.</param>
        public void SerializeObject(object data, string path) {

            if (data == null) {
                throw new ArgumentException("Data cannot be null");
            }
            var serializer = new JsonSerializer();

            using (var sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw)) {
                serializer.Serialize(writer, data);
            }
        }

        /// <summary>
        /// Deserialize JSON.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>Deserialized object</returns>
        public T DeserializeObject<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
