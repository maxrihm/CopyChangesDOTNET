using CopyChanges.Commands;
using CopyChanges.Services;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace CopyChanges.ViewModels
{
    public class ApplyChangesViewModel : BaseViewModel
    {
        private readonly IFileService _fileService;
        private readonly string _projectDirectory;

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

        public ApplyChangesViewModel(IFileService fileService, string projectDirectory)
        {
            _fileService = fileService;
            _projectDirectory = projectDirectory;
            ApplyChangesCommand = new RelayCommand(ApplyChanges);
        }

        private void ApplyChanges(object parameter)
        {
            if (string.IsNullOrEmpty(InputText)) return;

            var lines = InputText.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            string currentFilePath = null;
            var fileContentBuilder = new StringBuilder();

            foreach (var line in lines)
            {
                var match = Regex.Match(line, "^//\\s*(.*)$");
                if (match.Success)
                {
                    if (currentFilePath != null && fileContentBuilder.Length > 0)
                    {
                        SaveFileContent(currentFilePath, fileContentBuilder.ToString());
                        fileContentBuilder.Clear();
                    }
                    currentFilePath = match.Groups[1].Value;
                }
                else if (!string.IsNullOrEmpty(currentFilePath))
                {
                    fileContentBuilder.AppendLine(line);
                }
            }

            if (currentFilePath != null && fileContentBuilder.Length > 0)
            {
                SaveFileContent(currentFilePath, fileContentBuilder.ToString());
            }
        }

        private void SaveFileContent(string relativePath, string content)
        {
            var fullPath = System.IO.Path.Combine(_projectDirectory, relativePath);
            _fileService.WriteFileContent(fullPath, content);
        }
    }
}