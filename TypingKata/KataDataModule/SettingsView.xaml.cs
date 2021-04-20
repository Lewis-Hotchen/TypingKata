using System.Windows.Controls;
using KataDataModule.Interfaces;
using KataIocModule;

namespace KataDataModule {
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl {
        public SettingsView() {
            InitializeComponent();
            DataContext = new SettingsViewModel(
                BootStrapper.Resolve<ITinyMessengerHub>(),
                BootStrapper.Resolve<ISettingsRepository>(),
                BootStrapper.Resolve<ITypingResultsRepository>());
        }
    }
}
