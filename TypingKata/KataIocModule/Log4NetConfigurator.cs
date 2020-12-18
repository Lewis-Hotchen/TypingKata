namespace KataIocModule {

    /// <summary>
    /// This interface acts as a middleware to call the Log4NetConfigurator so that it can be injected into classes for
    /// testing purposes.
    /// </summary>
    public class Log4NetConfigurator : ILog4NetConfigurator {

        /// <summary>
        /// Calls the Log4Net XML Configurator.
        /// </summary>
        public void Configure() {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}