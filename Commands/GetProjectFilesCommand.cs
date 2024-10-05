using CopyChanges.Services;
using CopyChanges.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace CopyChanges.Commands
{
    public class GetProjectFilesCommand : ICommand
    {
        private readonly IFileService _fileService;
        private readonly MainViewModel _viewModel;

        public GetProjectFilesCommand(MainViewModel viewModel, IFileService fileService)
        {
            _viewModel = viewModel;
            _fileService = fileService;

            // Register to listen for changes in ProjectDirectory
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(MainViewModel.ProjectDirectory))
                {
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            };
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.ProjectDirectory);
        }

        public void Execute(object parameter)
        {
            var projectDirectory = _viewModel.ProjectDirectory;
            if (string.IsNullOrEmpty(projectDirectory)) return;

            var allFiles = _fileService.GetAllFiles(projectDirectory);
            List<string> ignorePatterns = new List<string>();

            // Check if .filesignore exists and add its patterns
            var filesIgnorePath = Path.Combine(projectDirectory, ".filesignore");
            if (File.Exists(filesIgnorePath))
            {
                ignorePatterns.AddRange(_fileService.ReadFileContent(filesIgnorePath)?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>());
            }

            // Check if .gitignore exists and add its patterns
            var gitIgnorePath = Path.Combine(projectDirectory, ".gitignore");
            if (File.Exists(gitIgnorePath))
            {
                ignorePatterns.AddRange(File.ReadAllLines(gitIgnorePath));
            }

            var filteredFiles = allFiles.Where(file => !_fileService.IsFileIgnored(file, ignorePatterns));
            var result = string.Join("\n", filteredFiles);

            // Update ViewModel properties
            _viewModel.ProjectFiles = result;

            if (_viewModel.TextEditors.Count > 0)
            {
                _viewModel.TextEditors[0].Content = result;
            }

            _viewModel.StatusMessage = "Project files loaded into editor 1.";
        }

        public event EventHandler CanExecuteChanged;
    }
}