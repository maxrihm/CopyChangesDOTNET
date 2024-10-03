using System.Collections.ObjectModel;

namespace CopyChanges.Models
{
    public class FileItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsChecked { get; set; }
        public ObservableCollection<FileItem> Children { get; set; }
    }
}
