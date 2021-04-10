using KataIocModule;

namespace KataDataModule.EventArgs
{
    public class ToggleSettingUpdated : GenericTinyMessage<bool>
    {
        public ToggleSettingUpdated(object sender, bool content) : base(sender, content) { }
    }
}
