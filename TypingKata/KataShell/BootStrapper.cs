using System;
using System.Configuration;
using System.Xml;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using KataTrainer;
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


        private static IModuleRegistrar LoadModules(ContainerBuilder builder) {

            //get assemblies of Modules - for this project only a few modules will be loaded
            //but if ever expanded this process should be dynamically loaded.
            var kataTrainerAssembly = typeof(KataTrainerModule).Assembly;

            //Register Modules without parameters
            //Return a registrar to add more modules at runtime if needed.
            return builder.RegisterAssemblyModules(kataTrainerAssembly);
        }

        public static T Resolve<T>(ILifetimeScope scope) {
            return scope.Resolve<T>();
        }

        public static T Resolve<T>(ILifetimeScope scope, Parameter[] parameters) {
            return scope.Resolve<T>(parameters);
        }   
    }
}   