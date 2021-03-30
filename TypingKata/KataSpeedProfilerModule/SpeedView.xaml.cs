using System.Windows.Controls;
using KataDataModule;
using KataIocModule;
using TinyMessenger;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Interaction logic for SpeedView.xaml
    /// </summary>
    public partial class SpeedView : UserControl {
        public SpeedView() {
            InitializeComponent();
            DataContext = new SpeedViewModel(BootStrapper.Resolve<ITypingProfilerFactory>(), BootStrapper.Resolve<ITinyMessengerHub>(), new DataSerializer());
        }
    }
}