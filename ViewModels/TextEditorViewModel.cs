using CopyChanges.Commands;
using System.Windows;
using System.Windows.Input;

namespace CopyChanges.ViewModels
{
    public class TextEditorViewModel : BaseViewModel
    {
        private string _content;
        public string Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(); }
        }

        public ICommand CopyContentCommand { get; }

        public TextEditorViewModel()
        {
            CopyContentCommand = new RelayCommand(CopyContent);
        }

        private void CopyContent(object parameter)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                System.Windows.Clipboard.SetText(Content);
            }
        }

    }
}
