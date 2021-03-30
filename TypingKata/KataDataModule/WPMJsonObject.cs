using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KataDataModule {

    [JsonObject]
    public class WPMJsonObject {

        [JsonProperty("Wpm")]
        public int Wpm { get; set; }
        [JsonProperty("Errors")]
        public int Errors { get; }
        [JsonProperty("ErrorRate")]
        public double ErrorRate { get; }
        [JsonProperty("ErrorWords")]
        public List<(string, string)> IncorrectWords { get; }
        [JsonProperty("Date")]
        public DateTime Date { get; }
        [JsonProperty("Time")]
        public double Time { get; }

        public WPMJsonObject(int wpm, int errors, double errorRate, List<(string, string)> incorrectWords, DateTime date, double time) {
            Wpm = wpm;
            Errors = errors;
            ErrorRate = errorRate;
            IncorrectWords = incorrectWords;
            Date = date;
            Time = time;
        }

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
