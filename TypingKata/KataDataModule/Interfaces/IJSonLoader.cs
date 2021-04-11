namespace KataDataModule.Interfaces {
    public interface IJSonLoader {
        T LoadTypeFromJson<T>(string file);
        void RefreshJsonFiles();
    }
}