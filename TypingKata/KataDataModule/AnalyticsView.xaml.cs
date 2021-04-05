using System;
using System.Windows.Controls;
using Autofac;
using Autofac.Core;
using KataIocModule;

namespace KataDataModule {
    /// <summary>
    /// Interaction logic for AnalyticsView.xaml
    /// </summary>
    public partial class AnalyticsView : UserControl {
        public AnalyticsView() {
            InitializeComponent();
            DataContext = new AnalyticsViewModel(BootStrapper.Resolve<IJSonLoader>(new Parameter[]{new NamedParameter("directory", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + KataDataModule.Resources.TypingKataData), new NamedParameter("messengerHub", BootStrapper.Resolve<ITinyMessengerHub>())}), BootStrapper.Resolve<ITinyMessengerHub>());
        }
    }
}
