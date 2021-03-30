using Autofac;

namespace KataDataModule {
    public class DataModule : Module {

        protected override void Load(ContainerBuilder builder) {

            builder.RegisterType<DataSerializer>().As<IDataSerializer>();

            base.Load(builder);
        }
    }
}
