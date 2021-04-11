﻿using System;
using System.IO.Abstractions;
using System.Windows.Controls;
using Autofac;
using Autofac.Core;
using KataDataModule.Interfaces;
using KataIocModule;

namespace KataDataModule {
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl {
        public SettingsView() {
            InitializeComponent();
            DataContext = new SettingsViewModel(BootStrapper.Resolve<IDataSerializer>(),
                BootStrapper.Resolve<IJSonLoader>(new Parameter[] {
                    new NamedParameter("directory", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + KataDataModule.Resources.TypingKataData),
                    new NamedParameter("messengerHub", BootStrapper.Resolve<ITinyMessengerHub>()),
                    new NamedParameter("dataSerializer", BootStrapper.Resolve<IDataSerializer>()),
                    new NamedParameter("fileSystem", BootStrapper.Resolve<IFileSystem>()) 
                }),
                BootStrapper.Resolve<ITinyMessengerHub>());
        }
    }
}
