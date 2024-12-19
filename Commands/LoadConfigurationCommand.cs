using CopyChanges.Interfaces;
using CopyChanges.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CopyChanges.Commands
{
    public class LoadConfigurationCommand : ICommand
    {
        private readonly MainViewModel _viewModel;
        private readonly IConfigService _configService;
        private readonly IMessageService _messageService;

        public LoadConfigurationCommand(MainViewModel viewModel, IConfigService configService, IMessageService messageService)
        {
            _viewModel = viewModel;
            _configService = configService;
            _messageService = messageService;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Select a Configuration File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var editorsContent = _configService.LoadConfiguration(openFileDialog.FileName);
                    if (editorsContent.Count != _viewModel.TextEditors.Count)
                    {
                        _messageService.ShowError("Configuration file does not match the expected number of text editors.");
                        return;
                    }

                    for (int i = 0; i < _viewModel.TextEditors.Count; i++)
                    {
                        _viewModel.TextEditors[i].Content = editorsContent[i];
                    }

                    _messageService.ShowStatusMessage("Configuration loaded successfully.");
                }
                catch (Exception ex)
                {
                    _messageService.ShowError($"Failed to load configuration: {ex.Message}");
                }
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
