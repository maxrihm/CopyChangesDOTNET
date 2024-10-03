using CopyChanges.Commands;
using CopyChanges.Helpers;
using CopyChanges.LineHandlers;
using CopyChanges.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CopyChanges.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IFileService _fileService;
        private readonly IGitService _gitService;
        private readonly IJsonService _jsonService;
        private readonly IClipboardService _clipboardService;
        private BaseLineHandler _lineHandlerChain;

        public ObservableCollection<TextEditorViewModel> TextEditors { get; }

        public ICommand BrowseProjectDirectoryCommand { get; }
        public ICommand GetGitChangesCommand { get; }

        private string _projectDirectory;
        public string ProjectDirectory
        {
            get => _projectDirectory;
            set
            {
                _projectDirectory = value;
                OnPropertyChanged();
                UpdateCurrentProjectLabel();
                SetupLineHandlerChain();  // Update line handler chain when project directory changes
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private string _currentProjectLabel;
        public string CurrentProjectLabel
        {
            get => _currentProjectLabel;
            private set
            {
                _currentProjectLabel = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(IFileService fileService, IGitService gitService, IJsonService jsonService, IClipboardService clipboardService)
        {
            _fileService = fileService;
            _gitService = gitService;
            _jsonService = jsonService;
            _clipboardService = clipboardService;

            TextEditors = new ObservableCollection<TextEditorViewModel>();

            for (int i = 0; i < 9; i++)
            {
                TextEditors.Add(new TextEditorViewModel(_lineHandlerChain, clipboardService));
            }

            BrowseProjectDirectoryCommand = new RelayCommand(BrowseProjectDirectory);
            GetGitChangesCommand = new RelayCommand(GetGitChanges, CanExecuteGitCommands);
        }

        private void SetupLineHandlerChain()
        {
            if (!string.IsNullOrEmpty(ProjectDirectory))
            {
                // Ensure the file line handler is initialized with the project directory
                var fileLineHandler = new FileLineHandler(_fileService, ProjectDirectory);
                var textLineHandler = new TextLineHandler();

                fileLineHandler.SetNext(textLineHandler);  // FileLineHandler should be first in the chain
                _lineHandlerChain = fileLineHandler;

                // Update the line handler chain for each editor
                foreach (var editor in TextEditors)
                {
                    editor.UpdateLineHandlerChain(_lineHandlerChain);
                }
            }
        }

        private void BrowseProjectDirectory(object parameter)
        {
            var dialogService = new DialogService();
            var directory = dialogService.OpenFolderDialog();

            if (!string.IsNullOrEmpty(directory))
            {
                ProjectDirectory = directory;
            }
            else
            {
                StatusMessage = "No directory selected.";
            }
        }

        private bool CanExecuteGitCommands(object parameter)
        {
            return !string.IsNullOrEmpty(ProjectDirectory);
        }

        private void GetGitChanges(object parameter)
        {
            var changes = _gitService.GetGitChanges(ProjectDirectory);

            if (TextEditors.Count > 0)
            {
                TextEditors[0].Content = string.Join("\n", changes);
            }

            StatusMessage = "Git changes loaded into editor 1.";
        }

        private void UpdateCurrentProjectLabel()
        {
            CurrentProjectLabel = string.IsNullOrEmpty(ProjectDirectory) ? "No project loaded" : $"Current Project: {ProjectDirectory}";
        }
    }
}
