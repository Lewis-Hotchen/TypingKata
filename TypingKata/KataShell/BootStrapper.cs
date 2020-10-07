using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using log4net;

namespace TypingKata {
    public static class BootStrapper {
        private static readonly ILog Log = LogManager.GetLogger("AppLog");
        private static RootViewModel _rootViewModel;

        private static IModuleRegistrar moduleRegistrar;

        public static IContainer Container { get; private set; }

        public static RootViewModel RootViewModel => _rootViewModel;

        private static readonly ILog log = LogManager.GetLogger(typeof(BootStrapper));

        public static void Start() {

            //Configure App.Config to set log path to documents
            ConfigureLog();

            Log.Info("BootStrapper Starting...");

            var builder = new ContainerBuilder();

            builder.RegisterType(typeof(RootViewModel));

            Log.Info("Loading Modules...");
            moduleRegistrar = LoadModules(builder);
            Log.Info("Modules Loaded");
                
            Container = builder.Build();

            Log.Info("Container Built");

            using (var scope = Container.BeginLifetimeScope()) {
                Log.Info("Starting Scope");
                _rootViewModel = Resolve<RootViewModel>(scope);
            }
        }

        private static void ConfigureLog() {
            //configure log path here.
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

            return builder.RegisterAssemblyModules(assemblies.ToArray());
        }

        /// <summary>
        /// Resolve a type with a given scope.
        /// </summary>
        /// <typeparam name="T">The interface to resolve.</typeparam>
        /// <param name="scope">The scope to resolve the interface from.</param>
        /// <returns>Resolved type.</returns>
        public static T Resolve<T>(ILifetimeScope scope) {
            return scope.Resolve<T>();
        }

        /// <summary>
        /// Resolve a type with a given scope with parameters.
        /// </summary>
        /// <typeparam name="T">The interface to resolve.</typeparam>
        /// <param name="scope">The scope to resolve the interface from.</param>
        /// <param name="parameters">Parameters to be passed if the concrete classes constructor is not parameterless.</param>
        /// <returns>Resolved type.</returns>
        public static T Resolve<T>(ILifetimeScope scope, Parameter[] parameters) {
            return scope.Resolve<T>(parameters);
        }   
    }
}   