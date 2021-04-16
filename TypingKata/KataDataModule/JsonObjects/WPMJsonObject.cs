using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KataDataModule.JsonObjects {

    /// <summary>
    /// Simple class to store json.
    /// </summary>
    [JsonObject]
    public class WPMJsonObject {

        [JsonProperty("Wpm")]
        public int Wpm { get; set; }
        [JsonProperty("Errors")]
        public int Errors { get; }
        [JsonProperty("ErrorRate")]
        public double ErrorRate { get; }
        [JsonProperty("ErrorWords")]
        public List<Tuple<string, string>> IncorrectWords { get; }
        [JsonProperty("Date")]
        public DateTime Date { get; }
        [JsonProperty("Time")]
        public double Time { get; }

        /// <summary>
        /// Create new WPMJsonObject.
        /// </summary>
        /// <param name="wpm"></param>
        /// <param name="errors"></param>
        /// <param name="errorRate"></param>
        /// <param name="incorrectWords"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        public WPMJsonObject(int wpm, int errors, double errorRate, List<Tuple<string, string>> incorrectWords, DateTime date, double time) {
            Wpm = wpm;
            Errors = errors;
            ErrorRate = Math.Round(errorRate, 2);
            IncorrectWords = incorrectWords;
            Date = date;
            Time = time;
        }

        /// <summary>
        /// Return the object as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("Errors: \n");
            foreach (var (item1, item2) in IncorrectWords) {
                sb.Append(item1 + " : " + item2 + "\n");
            }
            return $"WPM: {Wpm}. Errors made: {Errors}. Error Rate: %{ErrorRate}" + "\n" + sb;
        }

    }
}
