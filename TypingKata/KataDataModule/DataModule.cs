using System;
using System.IO.Abstractions;
using Autofac;
using KataDataModule.Interfaces;
using KataIocModule;

namespace KataDataModule {
    public class DataModule : Module {

        protected override void Load(ContainerBuilder builder) {


            builder.Register(
                (c, p) => new JsonLoader(p.Named<string>("directory"),
                    p.Named<ITinyMessengerHub>("messengerHub"),
                    p.Named<IDataSerializer>("dataSerializer"),
                    p.Named<IFileSystem>("fileSystem")))
                    .As<IJSonLoader>();
            builder.RegisterType<TinyMessengerHub>().As<ITinyMessengerHub>().InstancePerLifetimeScope();
            builder.RegisterType<DataSerializer>().As<IDataSerializer>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();

            base.Load(builder);
        }
    }
}