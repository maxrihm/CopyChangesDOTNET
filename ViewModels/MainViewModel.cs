// ViewModels/MainViewModel.cs
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

        public MainViewModel(IFileService fileService, IGitService gitService, IJsonService jsonService)
        {
            _fileService = fileService;
            _gitService = gitService;
            _jsonService = jsonService;

            TextEditors = new ObservableCollection<TextEditorViewModel>();

            for (int i = 0; i < 9; i++)
            {
                TextEditors.Add(new TextEditorViewModel());
            }

            BrowseProjectDirectoryCommand = new RelayCommand(BrowseProjectDirectory);
            GetGitChangesCommand = new RelayCommand(GetGitChanges, CanExecuteGitCommands);

            SetupLineHandlerChain();
        }

        private void SetupLineHandlerChain()
        {
            var fileLineHandler = new FileLineHandler(_fileService);
            var textLineHandler = new TextLineHandler();

            textLineHandler.SetNext(fileLineHandler);
            _lineHandlerChain = textLineHandler;
        }

        private void BrowseProjectDirectory(object parameter)
        {
            var dialogService = new DialogService();
            var directory = dialogService.OpenFolderDialog();

            if (!string.IsNullOrEmpty(directory))
            {
                ProjectDirectory = directory;
                // Load the files from the directory
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
    }
}
