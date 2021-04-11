namespace KataDataModule.Interfaces {
    public interface IDataSerializer {

        /// <summary>
        /// Serialize an object.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="path">Json File path.</param>
        void SerializeObject(object data, string path);

        /// <summary>
        /// Deserialize JSON.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>Deserialized object</returns>
        T DeserializeObject<T>(string json);
    }
}