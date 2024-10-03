using CopyChanges.Models;
using CopyChanges.Services;
using System.Collections.ObjectModel;
using System.IO;

namespace CopyChanges.ViewModels
{
    public class FileBrowserViewModel : BaseViewModel
    {
        private readonly IFileService _fileService;
        private readonly IJsonService _jsonService;

        public ObservableCollection<FileItem> Files { get; set; }

        public FileBrowserViewModel(IFileService fileService, IJsonService jsonService)
        {
            _fileService = fileService;
            _jsonService = jsonService;
            Files = new ObservableCollection<FileItem>();
        }

        public void LoadDirectory(string directory)
        {
            Files.Clear();
            var rootItem = new FileItem
            {
                Name = Path.GetFileName(directory),
                FullPath = directory,
                Children = GetDirectoryItems(directory)
            };
            Files.Add(rootItem);
        }

        private ObservableCollection<FileItem> GetDirectoryItems(string path)
        {
            var items = new ObservableCollection<FileItem>();
            foreach (var dir in Directory.GetDirectories(path))
            {
                items.Add(new FileItem
                {
                    Name = Path.GetFileName(dir),
                    FullPath = dir,
                    Children = GetDirectoryItems(dir)
                });
            }

            foreach (var file in Directory.GetFiles(path))
            {
                items.Add(new FileItem
                {
                    Name = Path.GetFileName(file),
                    FullPath = file,
                    IsChecked = false
                });
            }

            return items;
        }
    }
}
