using System.Collections.Generic;
using System.IO;
using KataIocModule;
using System.Linq;

namespace KataDataModule {
    public class JsonLoader : IJSonLoader {
        private List<(string path, string filename)> _files;

        public void RefreshJsonFiles(string directory) {
            _files = Directory.EnumerateFiles(directory, "*.json").Select(file => (File.ReadAllText(file), Path.GetFileName(file))).ToList();
        }

        public JsonLoader(string directory) {
            _files = Directory.EnumerateFiles(directory, "*.json").Select(file => (File.ReadAllText(file), Path.GetFileName(file))).ToList();
        }

        public T LoadTypeFromJson<T>(string file) {
            var serializer = BootStrapper.Resolve<IDataSerializer>();

            foreach (var (path, filename) in _files) {
                if (filename == file) {
                   return serializer.DeserializeObject<T>(File.ReadAllText(path));
                }
            }

            return default(T);
        }
    }
}