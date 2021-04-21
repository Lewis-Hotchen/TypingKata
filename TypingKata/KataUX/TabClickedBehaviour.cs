using System.Windows.Controls;
using System.Windows.Interactivity;
using KataIocModule;

namespace KataUX {

    public class TabClickedBehaviour : Behavior<TabControl> {

        protected override void OnAttached() {
            AssociatedObject.SelectionChanged += AssociatedObjectOnSelectionChanged;
        }

        private void AssociatedObjectOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            var messengerHub = BootStrapper.Resolve<ITinyMessengerHub>();
            messengerHub.Publish(new TabControlChangedMessage(this));
        }
    }
}
