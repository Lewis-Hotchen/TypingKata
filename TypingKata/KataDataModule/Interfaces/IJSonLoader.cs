namespace KataDataModule.Interfaces {
    public interface IJsonLoader {
        T LoadTypeFromJson<T>(string file);
        T LoadTypeFromJsonFile<T>(string path);
        void RefreshJsonFiles();
    }
}