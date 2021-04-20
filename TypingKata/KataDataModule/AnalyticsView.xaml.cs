using System;
using System.IO.Abstractions;
using System.Windows.Controls;
using Autofac;
using Autofac.Core;
using KataDataModule.Interfaces;
using KataIocModule;

namespace KataDataModule {
    /// <summary>
    /// Interaction logic for AnalyticsView.xaml
    /// </summary>
    public partial class AnalyticsView : UserControl {
        public AnalyticsView() {
            InitializeComponent();
            DataContext = new AnalyticsViewModel(BootStrapper.Resolve<ITinyMessengerHub>(),
                BootStrapper.Resolve<ITypingResultsRepository>());
        }
    }
}
