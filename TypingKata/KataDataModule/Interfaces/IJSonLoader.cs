namespace KataDataModule.Interfaces {
    public interface IJSonLoader {
        T LoadTypeFromJson<T>(string file);
        T LoadTypeFromJsonFile<T>(string path);
        void RefreshJsonFiles();
    }
}