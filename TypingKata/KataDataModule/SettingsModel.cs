using System;
using System.Collections.Generic;
using System.Linq;
using KataDataModule.Interfaces;
using KataDataModule.JsonObjects;

namespace KataDataModule {
    public class SettingsModel {

        private IDataSerializer _serializer;

        public SettingsModel(IDataSerializer serializer) {
            _serializer = serializer;
        }


    }
}