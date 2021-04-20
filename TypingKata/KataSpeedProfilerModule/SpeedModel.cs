using Autofac;
using Autofac.Core;
using KataIocModule;
using KataSpeedProfilerModule.Interfaces;
using log4net;

namespace KataSpeedProfilerModule {
    public class SpeedModel {
        private readonly ITypingProfilerFactory _profilerFactory;
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");

        public ITypingProfiler TypingProfiler { get; set; }

        public SpeedModel(ITypingProfilerFactory profilerFactory) {
            _profilerFactory = profilerFactory;
        }

        public ITypingProfiler ConstructProfiler(double testTime) {
            TypingProfiler = _profilerFactory.CreateTypingProfiler(
                BootStrapper.Resolve<ICursor>(),
                BootStrapper.Resolve<ITypingSpeedCalculator>(),
                BootStrapper.Resolve<ITypingTimer>(new Parameter[] { new NamedParameter("time", testTime) }),
                BootStrapper.Resolve<ITinyMessengerHub>());
            return TypingProfiler;
        }

    }
}