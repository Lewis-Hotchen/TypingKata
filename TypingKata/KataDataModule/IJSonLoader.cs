namespace KataDataModule {
    public interface IJSonLoader {
        T LoadTypeFromJson<T>(string file);
    }
}