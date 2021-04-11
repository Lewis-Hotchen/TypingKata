using System.Windows.Controls;
using KataDataModule.Interfaces;
using KataIocModule;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Interaction logic for SpeedView.xaml
    /// </summary>
    public partial class SpeedView : UserControl {
        public SpeedView() {
            InitializeComponent();
            DataContext = new SpeedViewModel(
                BootStrapper.Resolve<ITypingProfilerFactory>(),
                BootStrapper.Resolve<ITinyMessengerHub>(),
                BootStrapper.Resolve<IDataSerializer>(),
                BootStrapper.Resolve<ISettingsRepository>());
        }
    }
}