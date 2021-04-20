using System;
using System.Collections.Generic;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;

namespace KataDataModule {
    public class TypingResultsRepository : ITypingResultsRepository {

        private readonly IJSonLoader _jsonLoader;
        private readonly IDataSerializer _dataSerializer;
        private readonly string _path;
        private List<WPMJsonObject> _results;
        public event EventHandler ResultsChangedEvent;

        /// <summary>
        /// Collection of the results.
        /// </summary>
        public IEnumerable<WPMJsonObject> Results => _results;

        /// <summary>
        /// Instantiate new TypingResultsRepository.
        /// </summary>
        /// <param name="jsonLoader">The JsonLoader.</param>
        /// <param name="dataSerializer">The data serializer.</param>
        /// <param name="path">The file path.</param>
        public TypingResultsRepository(IJSonLoader jsonLoader, IDataSerializer dataSerializer, string path) {
            _jsonLoader = jsonLoader;
            _dataSerializer = dataSerializer;
            _path = path;
            _results = _jsonLoader.LoadTypeFromJson<List<WPMJsonObject>>(Resources.TypingResults) ?? new List<WPMJsonObject>();

            if (_results.Count == 0) {
                dataSerializer.SerializeObject(_results, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                           @"\" + Resources.TypingKataData + @"\" + Resources.TypingResults);
            }
        }

        /// <summary>
        /// Add a result.
        /// </summary>
        /// <param name="jsonObject">The result to add.</param>
        public void AddResult(WPMJsonObject jsonObject) {
            _results?.Add(jsonObject);
            WriteOutResults();
        }

        /// <summary>
        /// Remove a result.
        /// </summary>
        /// <param name="jsonObject">The json object to remove</param>
        public void RemoveResult(WPMJsonObject jsonObject) {
            if (_results.Remove(jsonObject)) {
                WriteOutResults();
            }
        }

        /// <summary>
        /// Remove a result by index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveResult(int index) {
            _results.RemoveAt(index);
            WriteOutResults();
        }

        /// <summary>
        /// Reset all results.
        /// </summary>
        public void ResetResults() {
            _results = new List<WPMJsonObject>();
            WriteOutResults();
        }

        /// <summary>
        /// Write out the results to file.
        /// </summary>
        public void WriteOutResults() {
            _dataSerializer.SerializeObject(_results,_path + @"\" + Resources.TypingResults);
            ResultsChangedEvent?.Invoke(this, System.EventArgs.Empty);
        }
    }
}