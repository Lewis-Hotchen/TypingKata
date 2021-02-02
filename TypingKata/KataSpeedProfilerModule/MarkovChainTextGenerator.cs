﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KataSpeedProfilerModule.Properties;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Markov text generator.
    /// This implementation is from:
    /// https://rosettacode.org/wiki/Markov_chain_text_generator#C.23
    /// </summary>
    public static class MarkovChainTextGenerator {

        public static string Markov(string[] words, int keySize, int outputSize) {
            if (keySize < 1) throw new ArgumentException("Key size can't be less than 1");

            if (outputSize < keySize || words.Length < outputSize) {
                throw new ArgumentException("Output size is out of range");
            }

            var dict = new Dictionary<string, List<string>>();
            for (int i = 0; i < words.Length - keySize; i++) {
                var key = words.Skip(i).Take(keySize).Aggregate(Join);
                string value;
                if (i + keySize < words.Length) {
                    value = words[i + keySize];
                } else {
                    value = "";
                }

                if (dict.ContainsKey(key)) {
                    dict[key].Add(value);
                } else {
                    dict.Add(key, new List<string>() { value });
                }
            }

            var rand = new Random();
            var output = new List<string>();
            var n = 0;
            var rn = rand.Next(dict.Count);
            var prefix = dict.Keys.Skip(rn).Take(1).Single();
            output.AddRange(prefix.Split());

            while (true) {
                var suffix = dict[prefix];
                if (suffix.Count == 1) {
                    if (suffix[0] == "") {
                        return output.Aggregate(Join);
                    }
                    output.Add(suffix[0]);
                } else {
                    rn = rand.Next(suffix.Count);
                    output.Add(suffix[rn]);
                }
                if (output.Count >= outputSize) {
                    return output.Take(outputSize).Aggregate(Join);
                }
                n++;
                prefix = output.Skip(n).Take(keySize).Aggregate(Join);
            }
        }

        private static string Join(string a, string b) {
            return a + " " + b;
        }
    }
}
