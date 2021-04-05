using System.Collections.Generic;
using System.IO;
using KataIocModule;
using System.Linq;

namespace KataDataModule {

    /// <summary>
    /// Class for handling serializing and deserializing of json.
    /// </summary>
    public class JsonLoader : IJSonLoader {
        private readonly string _directory;
        private readonly ITinyMessengerHub _messengerHub;
        private List<(string content, string filename)> _files;

        /// <summary>
        /// Refresh the json files.
        /// </summary>
        private void RefreshJsonFiles() {
            _files = Directory.EnumerateFiles(_directory, "*.json").Select(file => (File.ReadAllText(file), Path.GetFileName(file))).ToList();
            _messengerHub.Publish(new JsonUpdatedMessage(this));
        }

        /// <summary>
        /// Create new JsonLoader.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="messengerHub"></param>
        public JsonLoader(string directory, ITinyMessengerHub messengerHub) {
            _directory = directory;
            _messengerHub = messengerHub;
            _files = Directory.EnumerateFiles(directory, "*.json").Select(file => (File.ReadAllText(file), Path.GetFileName(file))).ToList();
        }

        /// <summary>
        /// Load a specific type from a json file.
        /// </summary>
        /// <typeparam name="T">The type to use.</typeparam>
        /// <param name="file">The name of the file to read from.</param>
        /// <returns></returns>
        public T LoadTypeFromJson<T>(string file) {
            var serializer = BootStrapper.Resolve<IDataSerializer>();

            foreach (var (content, filename) in _files) {
                if (filename == file) {
                   return serializer.DeserializeObject<T>(content);
                }
            }

            return default(T);
        }
    }
}