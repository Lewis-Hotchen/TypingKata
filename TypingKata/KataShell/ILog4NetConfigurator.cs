namespace TypingKata {

    /// <summary>
    /// This interface acts as a middleware to call the Log4NetConfigurator so that it can be injected into classes for
    /// testing purposes.
    /// </summary>
    public interface ILog4NetConfigurator {

        /// <summary>
        /// Calls the Log4Net XML Configurator.
        /// </summary>
        void Configure();

    }
}