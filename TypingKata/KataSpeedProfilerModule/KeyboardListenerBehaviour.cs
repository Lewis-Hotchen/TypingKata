using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using KataSpeedProfilerModule.Interfaces;

namespace KataSpeedProfilerModule {
    public class KeyboardListenerBehaviour : Behavior<FrameworkElement> {

        public static readonly DependencyProperty TypingProfilerProperty = DependencyProperty.Register(
            "TypingProfiler", typeof(ITypingProfiler), typeof(KeyboardListenerBehaviour), new PropertyMetadata(default(ITypingProfiler)));

        public ITypingProfiler TypingProfiler {
            get { return (ITypingProfiler) GetValue(TypingProfilerProperty); }
            set { SetValue(TypingProfilerProperty, value); }
        }

        protected override void OnAttached() {
            AssociatedObject.KeyDown += AssociatedObjectOnKeyDown;
            base.OnAttached();
        }

        protected override void OnDetaching() {
            AssociatedObject.KeyDown -= AssociatedObjectOnKeyDown;
            base.OnDetaching();
        }

        private void AssociatedObjectOnKeyDown(object sender, KeyEventArgs e) {
            TypingProfiler.CharacterInput(e.Key);
            e.Handled = true;
        }
    }
}
