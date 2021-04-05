using KataIocModule;

namespace KataSpeedProfilerModule.EventArgs {
    public class TestCompleteMessage : GenericTinyMessage<TestCompleteEventArgs> {
        public TestCompleteMessage(object sender, TestCompleteEventArgs content) : base(sender, content) { }
    }
}
