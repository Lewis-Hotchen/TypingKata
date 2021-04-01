namespace KataDataModule {
    public interface IJSonLoader {
        void RefreshJsonFiles(string directory);
        T LoadTypeFromJson<T>(string file);
    }
}