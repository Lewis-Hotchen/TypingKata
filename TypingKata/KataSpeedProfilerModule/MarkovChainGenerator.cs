using System;
using System.IO;
using System.Reflection;
using MarkVSharp;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Class to use MarkVSharp library implementation of Markov chain generator.
    /// From: https://github.com/emacarie/MarkVSharp
    /// </summary>
    public class MarkovChainGenerator : IMarkovChainGenerator {

        private readonly string _path;
        private readonly GeneratorFacade _generator;

        public MarkovChainGenerator(string path) {
            _path = path;
            _generator = new GeneratorFacade(new MarkovGenerator(GetWordsFromResource()));
        }

        public string GetText(int noOfWords) {
            return _generator.GenerateWords(noOfWords);
        }

        /// <summary>
        /// Generate words from resource.
        /// The text found in this text document is generated from:
        /// https://www.blindtextgenerator.com/lorem-ipsum
        /// </summary>
        /// <returns>Enumerable string of words.</returns>
        private string GetWordsFromResource() {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(_path))
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException())) {
                return reader.ReadToEnd();
            }
        }
    }
}