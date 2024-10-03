using System.Windows;

namespace CopyChanges.Services
{
    public class ClipboardService : IClipboardService
    {
        public void SetText(string text)
        {
            Clipboard.SetText(text);
        }
    }
}