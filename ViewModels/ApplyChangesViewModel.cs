using CopyChanges.Commands;
using CopyChanges.Services;
using CopyChanges.Constants;
using System.Windows.Input;
using System.IO;

namespace CopyChanges.ViewModels
{
    public class ApplyChangesViewModel : BaseViewModel
    {
        private readonly IFileService _fileService;
        private readonly IClipboardService _clipboardService;

        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }

        public ICommand ApplyChangesCommand { get; }
        public ICommand CompareChangesCommand { get; }

        public ApplyChangesViewModel(IFileService fileService, IClipboardService clipboardService)
        {
            _fileService = fileService;
            _clipboardService = clipboardService;
            ApplyChangesCommand = new RelayCommand(ApplyChanges);
            CompareChangesCommand = new RelayCommand(CompareChanges);
        }

        private void ApplyChanges(object parameter)
        {
            if (string.IsNullOrEmpty(InputText)) return;

            if (File.Exists(PathConstants.File1Path))
            {
                var oldContent = _fileService.ReadFileContent(PathConstants.File1Path);
                _fileService.WriteFileContent(PathConstants.File2Path, oldContent);
            }

            _fileService.WriteFileContent(PathConstants.File1Path, InputText);
        }

        private void CompareChanges(object parameter)
        {
            if (File.Exists(PathConstants.File1Path) && File.Exists(PathConstants.File2Path))
            {
                var file1Content = _fileService.ReadFileContent(PathConstants.File1Path);
                var file2Content = _fileService.ReadFileContent(PathConstants.File2Path);

                InputText = file1Content == file2Content ? "Files are identical." : "Files are different.";
            }
            else
            {
                InputText = "One or both files do not exist.";
            }
        }
    }
}

