using GalaSoft.MvvmLight;
using KataIocModule;
using log4net;

namespace KataSpeedProfilerModule {
    public class SpeedViewModel : ViewModelBase {
        private static readonly ILog Log = LogManager.GetLogger("SpeedProfilerLog");
        private SpeedModel _model;
        public SpeedViewModel() {
            this._model = new SpeedModel(BootStrapper.Resolve<ICursor>());
            Log.Debug($"model is {_model}");
        }
    }
}
