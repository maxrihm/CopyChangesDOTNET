using System.Windows;
using ICSharpCode.AvalonEdit;

namespace CopyChanges.Views
{
    public static class TextEditorHelper
    {
        public static readonly DependencyProperty BindableDocumentTextProperty =
            DependencyProperty.RegisterAttached(
                "BindableDocumentText",
                typeof(string),
                typeof(TextEditorHelper),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableDocumentTextChanged)
            );

        public static string GetBindableDocumentText(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableDocumentTextProperty);
        }

        public static void SetBindableDocumentText(DependencyObject obj, string value)
        {
            obj.SetValue(BindableDocumentTextProperty, value);
        }

        private static void OnBindableDocumentTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextEditor textEditor)
            {
                var newText = e.NewValue as string;

                if (textEditor.Document == null)
                {
                    textEditor.Document = new ICSharpCode.AvalonEdit.Document.TextDocument();
                }

                if (textEditor.Document.Text != newText)
                {
                    textEditor.Document.Text = newText ?? string.Empty;
                }

                textEditor.TextChanged += (sender, args) =>
                {
                    if (GetBindableDocumentText(textEditor) != textEditor.Document.Text)
                    {
                        SetBindableDocumentText(textEditor, textEditor.Document.Text);
                    }
                };
            }
        }
    }
}
