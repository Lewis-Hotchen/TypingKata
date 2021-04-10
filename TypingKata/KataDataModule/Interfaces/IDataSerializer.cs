namespace KataDataModule.Interfaces {
    public interface IDataSerializer {
        void SerializeObject(object data, string path);
        T DeserializeObject<T>(string json);
    }
}