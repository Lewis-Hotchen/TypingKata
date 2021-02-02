using Autofac;

namespace KataSpeedProfilerModule {

    public class KataSpeedProfilerModule : Module {
        public KataSpeedProfilerModule() {
        }

        protected sealed override void Load(ContainerBuilder builder) {

            //Parameterless registrations
            builder.RegisterType<Cursor>().As<ICursor>();
            builder.RegisterType<WordStack>().As<IWordStack>();
            builder.RegisterType<GeneratedWord>().As<IWord>();
            builder.RegisterType<UserDefinedWord>().As<IWord>();


            //Parameter registrations
            builder.Register(
                (c, p) => new Cursor(p.Named<IWord>("firstWord"))
            ).As<ICursor>();
            builder.Register(
                (c, p) => new TypingTimer(p.Named<double>("time"))
                ).As<ITypingTimer>();
            base.Load(builder);
        }
    }
}
