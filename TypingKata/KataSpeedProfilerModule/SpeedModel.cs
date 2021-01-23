using log4net;

namespace KataSpeedProfilerModule {
    public class SpeedModel {
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        public ICursor Cursor { get; }
        public ITypingTimer TypingTimer { get; }
        public SpeedModel(ICursor cursor, ITypingTimer timer) {
            Cursor = cursor;
            TypingTimer = timer;
        }   
    }
}
