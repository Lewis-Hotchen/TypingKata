﻿using System.Windows.Controls;
using KataDataModule;
using KataDataModule.Interfaces;
using KataIocModule;
using KataSpeedProfilerModule.Interfaces;

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
                BootStrapper.Resolve<ISettingsRepository>(),
                BootStrapper.Resolve<ITypingResultsRepository>()
                );
        }
    }
}