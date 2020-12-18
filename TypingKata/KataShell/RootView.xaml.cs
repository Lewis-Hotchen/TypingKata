using System.Windows;
using KataIocModule;
using TypingKata;

namespace KataShell {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RootView : Window {
        public RootView() {
            InitializeComponent();
            DataContext = new RootViewModel(BootStrapper.Resolve<IContainerBuilderFacade>(BootStrapper.Container));
        }
    }
}
