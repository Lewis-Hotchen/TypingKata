using System;
using System.Collections.Generic;
using KataDataModule.JsonObjects;

namespace KataDataModule.Interfaces {

    /// <summary>
    /// Class to store and update settings.
    /// </summary>
    public interface ISettingsRepository {

        /// <summary>
        /// Collection of the Settings.
        /// </summary>
        IList<SettingJsonObject> Settings { get; }

        event EventHandler SettingsUpdatedEvent;

        /// <summary>
        /// Write out the settings.
        /// </summary>
        void WriteOutSettings();

        /// <summary>
        /// Get a Setting by string index.
        /// </summary>
        /// <param name="index">The name of the setting.</param>
        /// <returns></returns>
        SettingJsonObject this[string index] { get; }
    }
}