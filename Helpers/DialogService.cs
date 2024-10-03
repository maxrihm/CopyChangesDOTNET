using System.Windows.Forms;

namespace CopyChanges.Helpers
{
    public class DialogService
    {
        public string OpenFolderDialog()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select Project Directory";
                dialog.ShowNewFolderButton = false; // You can allow the user to create new folders if you want by setting this to true.

                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    return dialog.SelectedPath;
                }
            }
            return string.Empty;
        }
    }
}
