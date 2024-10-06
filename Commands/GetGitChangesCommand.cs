// Commands/GetGitChangesCommand.cs
using CopyChanges.Interfaces;
using CopyChanges.ViewModels;
using System;
using System.Windows.Input;

namespace CopyChanges.Commands
{
    public class GetGitChangesCommand : ICommand
    {
        private readonly IGitService _gitService;
        private readonly MainViewModel _viewModel;

        public GetGitChangesCommand(MainViewModel viewModel, IGitService gitService)
        {
            _viewModel = viewModel;
            _gitService = gitService;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.ProjectDirectory);
        }

        public void Execute(object parameter)
        {
            var changes = _gitService.GetGitChanges(_viewModel.ProjectDirectory);
            // Handle the changes and process them in the ViewModel.
        }

        public event EventHandler CanExecuteChanged;
    }
}
