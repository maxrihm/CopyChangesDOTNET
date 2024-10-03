using Microsoft.Win32;

namespace CopyChanges.Helpers
{
    public class DialogService
    {
        public string OpenFolderDialog()
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select Folder",
                ValidateNames = false
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                return System.IO.Path.GetDirectoryName(dialog.FileName);
            }
            return string.Empty;
        }
    }
}
