// Commands/GetProjectFilesCommand.cs
using CopyChanges.Services;
using CopyChanges.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using LibGit2Sharp;  // Using LibGit2Sharp to get files tracked by Git

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

            var trackedFiles = GetGitTrackedFiles(projectDirectory);
            var filteredFiles = ApplyFilesIgnoreFilter(projectDirectory, trackedFiles);
            var result = string.Join("\n", filteredFiles);

            _viewModel.ProjectFiles = result;

            if (_viewModel.TextEditors.Count > 0)
            {
                _viewModel.TextEditors[0].Content = result;
            }

            _viewModel.StatusMessage = "Project files loaded into editor 1.";
        }

        public event EventHandler CanExecuteChanged;

        private IEnumerable<string> GetGitTrackedFiles(string projectDirectory)
        {
            using var repo = new Repository(projectDirectory);
            return repo.Index.Select(entry => entry.Path).ToList();  // Return relative paths
        }

        private IEnumerable<string> ApplyFilesIgnoreFilter(string projectDirectory, IEnumerable<string> files)
        {
            var filesIgnorePath = Path.Combine(projectDirectory, ".filesignore");
            var ignorePatterns = new HashSet<string>();

            if (File.Exists(filesIgnorePath))
            {
                var lines = _fileService.ReadFileContent(filesIgnorePath).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    ignorePatterns.Add(line.Trim());
                }
            }

            return files.Where(file => !ignorePatterns.Contains(file));  // Filter files based on .filesignore patterns
        }
    }
}