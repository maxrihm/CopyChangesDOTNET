// TextEditorViewModel.cs
using CopyChanges.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CopyChanges.ViewModels
{
    public class TextEditorViewModel : BaseViewModel
    {
        private string _content;

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(); // This raises the PropertyChanged event for the UI to update.
            }
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
