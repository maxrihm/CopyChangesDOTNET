using System.Windows.Controls;
using ICSharpCode.AvalonEdit;
using System.Windows;
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

        private void AvalonTextEditor_OnTextChanged(object sender, System.EventArgs e)
        {
            var textEditor = sender as TextEditor;
            if (textEditor != null && textEditor.DataContext is TextEditorViewModel viewModel)
            {
                viewModel.Content = textEditor.Text;
            }
        }
    }
}