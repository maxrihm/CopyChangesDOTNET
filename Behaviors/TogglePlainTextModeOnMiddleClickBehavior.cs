using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using ICSharpCode.AvalonEdit;
using CopyChanges.ViewModels;

namespace CopyChanges.Behaviors
{
    /// <summary>
    /// A behavior that toggles the associated text editor between
    /// normal mode and plain text mode when the middle mouse button is clicked.
    /// </summary>
    public class TogglePlainTextModeOnMiddleClickBehavior : Behavior<TextEditor>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseDown -= OnPreviewMouseDown;
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                if (AssociatedObject.DataContext is TextEditorViewModel vm)
                {
                    vm.IsPlainTextMode = !vm.IsPlainTextMode;
                }
            }
        }
    }
}
