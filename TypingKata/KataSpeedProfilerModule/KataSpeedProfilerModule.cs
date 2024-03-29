﻿using System;
using System.Configuration;
using System.Windows.Data;
using Autofac;
using KataDataModule;
using KataDataModule.Interfaces;
using KataIocModule;
using KataSpeedProfilerModule.Interfaces;
using KataSpeedProfilerModule.Properties;

namespace KataSpeedProfilerModule {

    public class KataSpeedProfilerModule : Module {

        protected sealed override void Load(ContainerBuilder builder) {

            var defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Resources.TypingKataData;
            const string _textPath = "KataSpeedProfilerModule.Resources.words.txt";
            //Parameter registrations
            builder.Register(
                (c, p) => new TypingTimer(p.Named<double>("time"))
            ).As<ITypingTimer>();

            builder.Register(
                (c, p) => new MarkovChainGenerator(_textPath)
            ).As<IMarkovChainGenerator>();

            builder.Register(
                    (c, p) => new SettingsRepository(defaultPath,
                        BootStrapper.Resolve<IDataSerializer>(),
                        BootStrapper.Resolve<IJsonLoader>()))
                .As<ISettingsRepository>().InstancePerLifetimeScope();

            //Parameterless registrations
            builder.RegisterType<TypingSpeedCalculator>().As<ITypingSpeedCalculator>();
            builder.RegisterType<Cursor>().As<ICursor>();
            builder.RegisterType<WordStack>().As<IWordStack>();
            builder.RegisterType<GeneratedWord>().As<IWord>().Keyed<IWord>("Generated");
            builder.RegisterType<UserDefinedWord>().As<IWord>().Keyed<IWord>("User");
            builder.RegisterType<KeyToCharacterConverter>().As<IValueConverter>().Keyed<IValueConverter>("KeyToChar");
            builder.RegisterType<TypingProfiler>().As<ITypingProfiler>().InstancePerLifetimeScope();
            builder.RegisterType<TypingProfilerFactory>().As<ITypingProfilerFactory>();
            builder.RegisterType<TinyMessengerHub>().As<ITinyMessengerHub>().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
