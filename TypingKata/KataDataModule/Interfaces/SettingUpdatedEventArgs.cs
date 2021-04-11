namespace KataDataModule.Interfaces {
    public class SettingUpdatedEventArgs : System.EventArgs {
        public string SettingName { get; }
        public object Value { get; }

        public SettingUpdatedEventArgs(string settingName, object value) {
            SettingName = settingName;
            Value = value;
        }

    }
}