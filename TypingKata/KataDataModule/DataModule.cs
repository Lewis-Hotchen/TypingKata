using Autofac;

namespace KataDataModule {
    public class DataModule : Module {

        protected override void Load(ContainerBuilder builder) {
            
            builder.RegisterType<DataSerializer>().As<IDataSerializer>();
            builder.RegisterType<JsonLoader>().As<IJSonLoader>();
            base.Load(builder);
        }
    }
}