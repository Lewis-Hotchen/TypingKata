using Autofac;
using log4net;

namespace KataSpeedProfilerModule {

    public class KataSpeedProfilerModule : Module {
        public KataSpeedProfilerModule() {
        }

        protected sealed override void Load(ContainerBuilder builder) {
            builder.RegisterType<Cursor>().As<ICursor>();

            //use p parameter in delegate to retrieve named parameter
            builder.Register(
                (c, p) => new TypingTimer(p.Named<double>("time"))
                ).As<ITypingTimer>();
            base.Load(builder);
        }
    }
}
