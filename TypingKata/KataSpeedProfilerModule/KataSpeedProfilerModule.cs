using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using KataIocModule;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {

    public class KataSpeedProfilerModule : Module {
        public KataSpeedProfilerModule() {
        }

        protected sealed override void Load(ContainerBuilder builder) {
            //Parameter registrations
            builder.Register(
                (c, p) => new TypingTimer(p.Named<double>("time"))
            ).As<ITypingTimer>();

            //Parameterless registrations
            builder.RegisterType<Cursor>().As<ICursor>();
            builder.RegisterType<WordStack>().As<IWordStack>();
            builder.RegisterType<WordQueue>().As<IWordQueue>();
            builder.RegisterType<GeneratedWord>().As<IWord>().Keyed<IWord>("Generated");
            builder.RegisterType<UserDefinedWord>().As<IWord>().Keyed<IWord>("User");
            builder.RegisterType<TypingProfiler>().As<ITypingProfiler>().InstancePerLifetimeScope();
            builder.RegisterType<TypingProfilerFactory>().As<ITypingProfilerFactory>();

            
            base.Load(builder);
        }
    }
}
