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
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.ProjectDirectory);
        }

        public void Execute(object parameter)
        {
            var files = GetProjectFiles(_viewModel.ProjectDirectory);
            _viewModel.ProjectFiles = string.Join("\n", files);
        }

        private IEnumerable<string> GetProjectFiles(string projectDirectory)
        {
            var allFiles = _fileService.GetAllFiles(projectDirectory);
            var filesIgnorePath = Path.Combine(projectDirectory, ".filesignore");

            var ignorePatterns = _fileService.ReadFileContent(filesIgnorePath)?.Split('\n') ?? Array.Empty<string>();
            var gitIgnorePath = Path.Combine(projectDirectory, ".gitignore");

            if (File.Exists(gitIgnorePath))
            {
                ignorePatterns = ignorePatterns.Concat(File.ReadAllLines(gitIgnorePath)).ToArray();
            }

            return allFiles.Where(file => !_fileService.IsFileIgnored(file, ignorePatterns));
        }

        public event EventHandler CanExecuteChanged;
    }
}

