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
            AssociatedObject.KeyUp += AssociatedObjectOnKeyDown;
            base.OnAttached();
        }

        protected override void OnDetaching() {
            AssociatedObject.KeyUp -= AssociatedObjectOnKeyDown;
            base.OnDetaching();
        }

        private void AssociatedObjectOnKeyDown(object sender, KeyEventArgs e) {

            if (e.Key == Key.Space) {
                TypingProfiler.CharacterInput(' ');
                e.Handled = true;
            }

            var c = KeyConverter.GetCharFromKey(e.Key);
            TypingProfiler.CharacterInput(c);
            e.Handled = true;
        }
    }
}