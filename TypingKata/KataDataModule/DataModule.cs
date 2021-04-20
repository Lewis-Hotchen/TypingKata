using System;
using System.IO.Abstractions;
using Autofac;
using KataDataModule.Interfaces;
using KataIocModule;

namespace KataDataModule {
    public class DataModule : Module {

        protected override void Load(ContainerBuilder builder) {

            var defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData;

            builder.Register(
                (c, p) => new JsonLoader(defaultPath,
                    BootStrapper.Resolve<ITinyMessengerHub>(),
                    BootStrapper.Resolve<IDataSerializer>(),
                    BootStrapper.Resolve<IFileSystem>()))
                    .As<IJsonLoader>();

            builder.Register(
                (c, p) => new TypingResultsRepository(BootStrapper.Resolve<IJsonLoader>(),
                    BootStrapper.Resolve<IDataSerializer>(),
                    defaultPath)
            ).As<ITypingResultsRepository>().InstancePerLifetimeScope();

            builder.Register(
                    (c, p) => new SettingsRepository(defaultPath,
                        BootStrapper.Resolve<IDataSerializer>(),
                        BootStrapper.Resolve<IJsonLoader>()))
                .As<ISettingsRepository>().InstancePerLifetimeScope();

            builder.RegisterType<TinyMessengerHub>().As<ITinyMessengerHub>().InstancePerLifetimeScope();
            builder.RegisterType<DataSerializer>().As<IDataSerializer>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();

            base.Load(builder);
        }
    }
}