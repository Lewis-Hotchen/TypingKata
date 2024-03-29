﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using KataIocModule;
using System.Linq;
using KataDataModule.EventArgs;
using KataDataModule.Interfaces;
using log4net;
using Newtonsoft.Json;

namespace KataDataModule {

    /// <summary>
    /// Class for handling serializing and deserializing of json.
    /// </summary>
    public class JsonLoader : IJsonLoader {
        private readonly string _directory;
        private readonly ITinyMessengerHub _messengerHub;
        private readonly IDataSerializer _serializer;
        private List<(string content, string filename)> _files;
        private readonly IFileSystem _fileSystem;
        private readonly ILog _log = LogManager.GetLogger(nameof(JsonLoader));

        /// <summary>
        /// Refresh the json files.
        /// </summary>
        public void RefreshJsonFiles() {
            _files = _fileSystem.Directory.EnumerateFiles(_directory, "*.json")
                .Select(file => (_fileSystem.File.ReadAllText(file), Path.GetFileName(file))).ToList();
            _messengerHub.Publish(new JsonUpdatedMessage(this));
        }


        /// <summary>
        /// Create new JsonLoader.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="messengerHub">The Tiny messenger hub.</param>
        /// <param name="serializer">The data serializer.</param>
        /// <param name="fileSystem">The File System.</param>
        public JsonLoader(string directory, 
            ITinyMessengerHub messengerHub, 
            IDataSerializer serializer,
            IFileSystem fileSystem) {
            _directory = directory;
            _messengerHub = messengerHub;
            _serializer = serializer;
            _fileSystem = fileSystem;
            _files = _fileSystem.Directory.EnumerateFiles(directory, "*.json")
                .Select(file => (fileSystem.File.ReadAllText(file), Path.GetFileName(file))).ToList();
        }

        /// <summary>
        /// Load a specific type from a json file.
        /// </summary>
        /// <typeparam name="T">The type to use.</typeparam>
        /// <param name="file">The name of the file to read from.</param>
        /// <returns>The loaded type, or the default of that type if failed.</returns>
        public T LoadTypeFromJson<T>(string file) {

            foreach (var (content, filename) in _files) {
                if (filename == file) {
                   return _serializer.DeserializeObject<T>(content);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Load type from a json file.
        /// </summary>
        /// <typeparam name="T">The type to load.</typeparam>
        /// <param name="file">The file to load.</param>
        /// <returns></returns>
        public T LoadTypeFromJsonFile<T>(string file) {
            var json = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                        @"\" + file);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}