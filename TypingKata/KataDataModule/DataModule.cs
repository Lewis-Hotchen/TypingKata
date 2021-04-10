using System;
using Autofac;
using KataDataModule.Interfaces;
using KataIocModule;

namespace KataDataModule {
    public class DataModule : Module {

        protected override void Load(ContainerBuilder builder) {

            builder.Register(
                (c, p) => new JsonLoader(p.Named<string>("directory"), p.Named<ITinyMessengerHub>("messengerHub")))
                    .As<IJSonLoader>();
            builder.RegisterType<TinyMessengerHub>().As<ITinyMessengerHub>().InstancePerLifetimeScope();
            builder.RegisterType<DataSerializer>().As<IDataSerializer>();

            base.Load(builder);
        }
    }
}