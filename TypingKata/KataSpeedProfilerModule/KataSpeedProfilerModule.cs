using Autofac;
using log4net;

namespace KataSpeedProfilerModule {

    public class KataSpeedProfilerModule : Module {
        public KataSpeedProfilerModule() {
            
        }

        protected sealed override void Load(ContainerBuilder builder) {
            builder.RegisterType<Cursor>().As<ICursor>();
            base.Load(builder);
        }
    }
}
