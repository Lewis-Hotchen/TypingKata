using System;
using System.Windows.Documents;
using System.Windows.Media;
using Autofac;
using Autofac.Core;
using KataIocModule;
using KataSpeedProfilerModule.EventArgs;
using KataSpeedProfilerModule.Interfaces;
using log4net;

namespace KataSpeedProfilerModule {
    public class SpeedModel {
        private readonly ITypingProfilerFactory _profilerFactory;
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        private const string TextPath = "KataSpeedProfilerModule.Resources.words.txt";

        public ITypingProfiler TypingProfiler { get; set; }

        public SpeedModel(ITypingProfilerFactory profilerFactory) {
            _profilerFactory = profilerFactory;
        }

        public ITypingProfiler ConstructProfiler(double testTime) {
            TypingProfiler = _profilerFactory.CreateTypingProfiler(
                BootStrapper.Resolve<ICursor>(),
                BootStrapper.Resolve<IWordStack>(),
                BootStrapper.Resolve<ITypingTimer>(new Parameter[] { new NamedParameter("time", testTime) }),
                BootStrapper.Resolve<IMarkovChainGenerator>(new Parameter[] { new NamedParameter("path", TextPath) }));
            return TypingProfiler;
        }

        public void ResetProfiler() {

        }
    }
}