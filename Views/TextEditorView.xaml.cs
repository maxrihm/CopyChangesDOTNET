using System.Windows.Controls;
using ICSharpCode.AvalonEdit;
using System.Windows.Data;
using CopyChanges.ViewModels;

namespace CopyChanges.Views
{
    public partial class TextEditorView : UserControl
    {
        public TextEditorView()
        {
            InitializeComponent();
        }

        private void AvalonTextEditor_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is TextEditor textEditor)
            {
                var textBinding = new Binding("Content")
                {
                    Source = textEditor.DataContext,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                textEditor.SetBinding(TextEditorHelper.BindableDocumentTextProperty, textBinding);
            }
        }
    }
}
