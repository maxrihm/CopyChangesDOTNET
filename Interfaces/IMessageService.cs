namespace CopyChanges.Interfaces
{
    public interface IMessageService
    {
        void ShowStatusMessage(string message);
        void ShowError(string errorMessage);
        event System.EventHandler<string> StatusMessageChanged;
        event System.EventHandler<string> ErrorMessageChanged;
    }
}
