using CopyChanges.Commands;
using CopyChanges.Helpers;
using CopyChanges.LineHandlers;
using CopyChanges.Services;
using CopyChanges.Views;
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
        public ICommand GetProjectFilesCommand { get; }
        public ICommand OpenApplyChangesWindowCommand { get; }
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
                TextEditors.Add(new TextEditorViewModel(_lineHandlerChain, clipboardService, i + 1));
            }

            BrowseProjectDirectoryCommand = new RelayCommand(BrowseProjectDirectory);
            GetGitChangesCommand = new RelayCommand(GetGitChanges, CanExecuteGitCommands);
            GetProjectFilesCommand = new GetProjectFilesCommand(this, _fileService);
            OpenApplyChangesWindowCommand = new RelayCommand(OpenApplyChangesWindow);
            TestCommand = new RelayCommand(TestMethod);
        }

        private void SetupLineHandlerChain()
        {
            if (!string.IsNullOrEmpty(ProjectDirectory))
            {
                var vscodeExtensionAllHandler = new VSCodeExtensionAllHandler(_jsonService, ProjectDirectory);
                var fileLineHandler = new FileLineHandler(_fileService, ProjectDirectory);
                var referenceLineHandler = new ReferenceLineHandler(TextEditors, vscodeExtensionAllHandler);
                var textLineHandler = new TextLineHandler();

                vscodeExtensionAllHandler.SetNext(fileLineHandler);
                fileLineHandler.SetNext(referenceLineHandler);
                referenceLineHandler.SetNext(textLineHandler);

                _lineHandlerChain = vscodeExtensionAllHandler;

                foreach (var editor in TextEditors)
                {
                    editor.UpdateLineHandlerChain(_lineHandlerChain);
                }
            }
        }


        private void OpenApplyChangesWindow(object parameter)
        {
            var window = new ApplyChangesWindow
            {
                DataContext = new ApplyChangesViewModel(_fileService, ProjectDirectory)
            };
            window.Show();
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

        private void TestMethod(object parameter)
        {
            if (TextEditors.Count > 0)
            {
                TextEditors[0].Content = "abc";
            }
        }
    }
}
