using System;
using System.Linq;
using System.Windows.Input;
using CopyChanges.Interfaces;
using CopyChanges.ViewModels;
using Microsoft.Win32;
using System.Collections.Generic;

namespace CopyChanges.Commands
{
    public class SaveConfigurationCommand : ICommand
    {
        private readonly MainViewModel _viewModel;
        private readonly IConfigService _configService;
        private readonly IMessageService _messageService;

        public SaveConfigurationCommand(MainViewModel viewModel, IConfigService configService, IMessageService messageService)
        {
            _viewModel = viewModel;
            _configService = configService;
            _messageService = messageService;
        }

        public bool CanExecute(object parameter) => true;

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Save Configuration File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var editorsContent = new List<string>();
                    foreach (var editor in _viewModel.TextEditors)
                    {
                        editorsContent.Add(editor.Content ?? string.Empty);
                    }

                    _configService.SaveConfiguration(saveFileDialog.FileName, editorsContent);
                    _messageService.ShowStatusMessage("Configuration saved successfully.");
                }
                catch (Exception ex)
                {
                    _messageService.ShowError($"Failed to save configuration: {ex.Message}");
                }
            }
        }
    }
}
