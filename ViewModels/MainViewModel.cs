using CopyChanges.Commands;
using CopyChanges.Helpers;
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

        public FileBrowserViewModel FileBrowserViewModel { get; }
        public ObservableCollection<TextEditorViewModel> TextEditors { get; }

        // Commands
        public ICommand BrowseProjectDirectoryCommand { get; }
        public ICommand GetGitChangesCommand { get; }

        // Properties
        private string _projectDirectory;
        public string ProjectDirectory
        {
            get => _projectDirectory;
            set { _projectDirectory = value; OnPropertyChanged(); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _fileService = new FileService();
            _gitService = new GitService();
            _jsonService = new JsonService();

            FileBrowserViewModel = new FileBrowserViewModel(_fileService, _jsonService);
            TextEditors = new ObservableCollection<TextEditorViewModel>();

            for (int i = 0; i < 9; i++)
            {
                TextEditors.Add(new TextEditorViewModel());
            }

            BrowseProjectDirectoryCommand = new RelayCommand(BrowseProjectDirectory);
            GetGitChangesCommand = new RelayCommand(GetGitChanges, CanExecuteGitCommands);
        }

        private void BrowseProjectDirectory(object parameter)
        {
            var dialogService = new DialogService();
            var directory = dialogService.OpenFolderDialog();

            if (!string.IsNullOrEmpty(directory))
            {
                ProjectDirectory = directory;
                FileBrowserViewModel.LoadDirectory(directory);
                StatusMessage = "Project directory loaded.";
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
            TextEditors[0].Content = string.Join("\n", changes);
            StatusMessage = "Git changes loaded into editor 1.";
        }
    }
}
