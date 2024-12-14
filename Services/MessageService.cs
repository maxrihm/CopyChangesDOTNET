using CopyChanges.Interfaces;
using System;

namespace CopyChanges.Services
{
    public class MessageService : IMessageService
    {
        public event EventHandler<string> StatusMessageChanged;
        public event EventHandler<string> ErrorMessageChanged;

        public void ShowStatusMessage(string message)
        {
            StatusMessageChanged?.Invoke(this, message);
        }

        public void ShowError(string errorMessage)
        {
            ErrorMessageChanged?.Invoke(this, errorMessage);
        }
    }
}
