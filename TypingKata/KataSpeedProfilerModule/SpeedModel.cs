using log4net;

namespace KataSpeedProfilerModule {
    public class SpeedModel {
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        public SpeedModel(ICursor cursor) {
            Log.Debug($"Cursor is {cursor}");
        }

    }
}
