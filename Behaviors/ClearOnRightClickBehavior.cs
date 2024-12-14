using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors; // Ensure you have Microsoft.Xaml.Behaviors.Wpf referenced
using ICSharpCode.AvalonEdit;
using CopyChanges.ViewModels;

namespace CopyChanges.Behaviors
{
    /// <summary>
    /// A behavior that, when attached to an AvalonEdit TextEditor,
    /// will clear the editor's content on right-click.
    /// This encourages separation of concerns and keeps UI logic out of the View and ViewModel.
    /// </summary>
    public class ClearOnRightClickBehavior : Behavior<TextEditor>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseRightButtonDown -= OnPreviewMouseRightButtonDown;
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get the ViewModel from DataContext
            if (AssociatedObject.DataContext is TextEditorViewModel vm)
            {
                // Execute ClearContentCommand if it can be executed
                if (vm.ClearContentCommand != null && vm.ClearContentCommand.CanExecute(null))
                {
                    vm.ClearContentCommand.Execute(null);
                }

                // Mark event as handled if desired
                // e.Handled = true; // Uncomment if you don't want any other right-click behavior
            }
        }
    }
}
