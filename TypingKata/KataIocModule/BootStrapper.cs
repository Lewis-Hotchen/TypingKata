using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using log4net;
using log4net.Core;

namespace KataIocModule {
    public static class BootStrapper {

        private static int noOfContainerBuilds;

        private static readonly ILog Log = LogManager.GetLogger("AppLog");
        private static IModuleRegistrar _moduleRegistrar;
        public static IContainer Container { get; private set; }
        public static ILog4NetConfigurator Log4NetConfigurator;
        public static ILifetimeScope Scope;

        public static void Start<TRoot>() {
            noOfContainerBuilds = 0;
            var builder = new ContainerBuilder();
            ConfigureContainer<TRoot>(builder);    
            ConfigureLog(Log4NetConfigurator);

            Log.Info("BootStrapper Starting...");
            Log.Info("Log configured");

            Scope = Container.BeginLifetimeScope();
            Log.Info("Starting Scope");
            Resolve<TRoot>();
        }

        private static void ConfigureContainer<TRoot>(ContainerBuilder builder) {
            Log.Debug("Container configuration called");
            builder.RegisterType(typeof(TRoot));
            builder.RegisterType(typeof(Log4NetConfigurator));
            builder.RegisterType<LogImpl>().As<ILog>();
            builder.RegisterType<ContainerBuilderFacade>().As<IContainerBuilderFacade>();
            Log.Info("Loading Modules...");
            _moduleRegistrar = LoadModules(builder);
            Log.Info("Modules Loaded");

            Container = builder.Build();
            noOfContainerBuilds++;
            Log.Debug($"Container built ({noOfContainerBuilds}) times, with ({Container.ComponentRegistry.Registrations.Count()}) number of types.");
        }

        /// <summary>
        /// Configures the log4net config.
        /// </summary>
        /// <param name="log4NetConfigurator">The configurator class that will call the configurator. Injected as dependency for testing purposes.</param>
        private static void ConfigureLog(ILog4NetConfigurator log4NetConfigurator) {
            log4NetConfigurator.Configure();
        }

        /// <summary>
        /// Loads all modules assemblies at Run time.
        /// This works by searching the output files of the executing assembly and looks for any .dll files ending with "Module"
        /// and loads any classes that inherit the AutoFac.Module class.
        /// In order to load an assembly module the project *must* end with Module or it won't find it and *must* be referenced in Shell.
        /// </summary>
        /// <param name="builder">The container builder that loads the modules.</param>
        /// <returns>Module registrar to enable dynamic loading of modules.</returns>
        private static IModuleRegistrar LoadModules(ContainerBuilder builder) {

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrWhiteSpace(path)) {
                return null;
            }

            var assemblies = Directory.GetFiles(path, "*Module.dll", SearchOption.TopDirectoryOnly)
                .Select(Assembly.LoadFrom);

            Log.Debug($"Found {assemblies.Count()} module assemblies");

            return builder.RegisterAssemblyModules(assemblies.ToArray());
        }

        /// <summary>
        /// Resolve a type with a given scope.
        /// </summary>
        /// <typeparam name="T">The interface to resolve.</typeparam>
        /// <returns>Resolved type.</returns>
        public static T Resolve<T>() {
            return Scope.Resolve<T>();
        }

        /// <summary>
        /// Resolve a type with a container.
        /// </summary>
        /// <typeparam name="T">The interface to resolve.</typeparam>
        /// <param name="container">The container to resolve the interface from.</param>
        /// <returns>Resolved type.</returns>
        public static T Resolve<T>(IContainer container) {
            return container.Resolve<T>();
        }

        /// <summary>
        /// Resolve a type with a given scope with parameters.
        /// </summary>
        /// <typeparam name="T">The interface to resolve.</typeparam>
        /// <param name="parameters">Parameters to be passed if the concrete classes constructor is not parameterless.</param>
        /// <returns>Resolved type.</returns>
        public static T Resolve<T>(Parameter[] parameters) {
            return Scope.Resolve<T>(parameters);
        }

        /// <summary>
        /// Register a type to the Parent container, will rebuild the container.
        /// </summary>
        /// <typeparam name="TImp">The type to register.</typeparam>
        /// <typeparam name="TInt">The interface to resolve the type with.</typeparam>
        /// <typeparam name="TRoot">The root view type.</typeparam>
        /// <remarks>Note from AutoFac on ContainerBuilder.Update being marked obsolete:
        /// Containers should generally be considered immutable. Register all of your dependencies before building/resolving.
        /// If you need to change the contents of a container, you technically should rebuild the container.
        /// As of my current running ver. of AutoFac 6.0.0 this method has been removed.
        /// </remarks>
        public static void RegisterType<TRoot, TImp, TInt>(IContainerBuilderFacade builder)
        {
            ConfigureContainer<TRoot>(builder.RegisterType<TInt, TImp>().Build().GetCachedBuilder());
        }
    }
}