using CopyChanges.Commands;
using CopyChanges.Helpers;
using CopyChanges.Interfaces;
using CopyChanges.LineHandlers;
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
        private readonly IMessageService _messageService;
        private readonly ILineHandlerChainFactory _lineHandlerChainFactory;

        private BaseLineHandler _lineHandlerChain;

        public ObservableCollection<TextEditorViewModel> TextEditors { get; }

        public ICommand BrowseProjectDirectoryCommand { get; }
        public ICommand GetGitChangesCommand { get; }
        public ICommand GetProjectFilesCommand { get; }
        public ICommand TestCommand { get; }

        private string _projectDirectory;
        public string ProjectDirectory
        {
            get => _projectDirectory;
            set
            {
                _projectDirectory = value;
                OnPropertyChanged();
                UpdateCurrentProjectLabel();
                SetupLineHandlerChain();
            }
        }

        private string _projectFiles;
        public string ProjectFiles
        {
            get => _projectFiles;
            set
            {
                _projectFiles = value;
                OnPropertyChanged();
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

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
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

        public MainViewModel(IFileService fileService,
                             IGitService gitService,
                             IJsonService jsonService,
                             IClipboardService clipboardService,
                             IMessageService messageService,
                             ILineHandlerChainFactory lineHandlerChainFactory)
        {
            _fileService = fileService;
            _gitService = gitService;
            _jsonService = jsonService;
            _clipboardService = clipboardService;
            _messageService = messageService;
            _lineHandlerChainFactory = lineHandlerChainFactory;

            TextEditors = new ObservableCollection<TextEditorViewModel>();

            // Initialize text editors with no chain yet. Will be updated after project directory is set.
            for (int i = 0; i < 9; i++)
            {
                TextEditors.Add(new TextEditorViewModel(null, _clipboardService, _messageService, i + 1));
            }

            BrowseProjectDirectoryCommand = new RelayCommand(BrowseProjectDirectory);
            GetGitChangesCommand = new RelayCommand(GetGitChanges, CanExecuteGitCommands);
            GetProjectFilesCommand = new GetProjectFilesCommand(this, _fileService);
            TestCommand = new RelayCommand(TestMethod);

            _messageService.StatusMessageChanged += (s, msg) => StatusMessage = msg;
            _messageService.ErrorMessageChanged += (s, msg) => ErrorMessage = msg;

            SetupLineHandlerChain();
        }

        private void SetupLineHandlerChain()
        {
            _lineHandlerChain = _lineHandlerChainFactory.CreateChain(ProjectDirectory, TextEditors);

            foreach (var editor in TextEditors)
            {
                editor.UpdateLineHandlerChain(_lineHandlerChain);
            }
        }

        private void BrowseProjectDirectory(object parameter)
        {
            var dialogService = new DialogService();
            var directory = dialogService.OpenFolderDialog();

            if (!string.IsNullOrEmpty(directory))
            {
                ProjectDirectory = directory;
                _messageService.ShowStatusMessage("Project directory selected.");
            }
            else
            {
                _messageService.ShowStatusMessage("No directory selected.");
            }
        }

        private bool CanExecuteGitCommands(object parameter)
        {
            return !string.IsNullOrEmpty(ProjectDirectory);
        }

        private void GetGitChanges(object parameter)
        {
            if (string.IsNullOrEmpty(ProjectDirectory))
            {
                _messageService.ShowError("No project directory selected.");
                return;
            }

            var changes = _gitService.GetGitChanges(ProjectDirectory);

            if (TextEditors.Count > 0)
            {
                TextEditors[0].Content = string.Join("\n", changes);
            }

            _messageService.ShowStatusMessage("Git changes loaded into editor 1.");
        }

        private void UpdateCurrentProjectLabel()
        {
            CurrentProjectLabel = string.IsNullOrEmpty(ProjectDirectory) ? "No project loaded" : $"Current Project: {ProjectDirectory}";
        }

        private void TestMethod(object parameter)
        {
            if (TextEditors.Count > 0)
            {
                TextEditors[0].Content = "abc";
                _messageService.ShowStatusMessage("Test content added to editor 1.");
            }
        }
    }
}
