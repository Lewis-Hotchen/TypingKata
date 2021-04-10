using KataIocModule;

namespace KataDataModule.EventArgs {

    /// <summary>
    /// Message to update json files.
    /// </summary>
    public class JsonUpdatedMessage : TinyMessageBase {
        public JsonUpdatedMessage(object sender) : base(sender) { }
    }
}