using System;
using System.Collections.Generic;
using KataDataModule.JsonObjects;

namespace KataDataModule {
    public interface ITypingResultsRepository {

        void AddResult(WPMJsonObject jsonObject);
        void RemoveResult(WPMJsonObject jsonObject);
        void WriteOutResults();
        void RemoveResult(int index);
        void ResetResults();
        IEnumerable<WPMJsonObject> Results { get; }
        event EventHandler ResultsChangedEvent;
    }
}