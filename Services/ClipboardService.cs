using System.Windows;
using CopyChanges.Interfaces;

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
