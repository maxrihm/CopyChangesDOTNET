using CopyChanges.Commands;
using CopyChanges.Helpers;
using CopyChanges.Interfaces;
using System.Windows.Input;

namespace CopyChanges.ViewModels
{
    public class FileBrowserViewModel : BaseViewModel
    {
        private readonly IFileService _fileService;
        private readonly MainViewModel _mainViewModel;

        public ICommand LoadProjectDirectoryCommand { get; }

        public FileBrowserViewModel(IFileService fileService, MainViewModel mainViewModel)
        {
            _fileService = fileService;
            _mainViewModel = mainViewModel;

            LoadProjectDirectoryCommand = new RelayCommand(LoadProjectDirectory);
        }

        private void LoadProjectDirectory(object parameter)
        {
            var dialogService = new DialogService();
            var directory = dialogService.OpenFolderDialog();

            if (!string.IsNullOrEmpty(directory))
            {
                _mainViewModel.ProjectDirectory = directory;
            }
        }
    }
}
