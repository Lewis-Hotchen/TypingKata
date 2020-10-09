using System.Windows;

namespace TypingKata {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {


        public App() {
            
            //Work around to allow some "dependency injection" in my BootStrapper class before the container is actually configured
            //testing purposes.
            BootStrapper.Log4NetConfigurator = new Log4NetConfigurator(); 
            BootStrapper.Start();
        }

    }
}
