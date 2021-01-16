using log4net;

namespace KataSpeedProfilerModule {
    public class SpeedModel {
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        public ICursor Cursor { get; }
        public SpeedModel(ICursor cursor) {
            Cursor = cursor;
            Log.Debug($"Cursor is {cursor}");
        }

    }
}
