using System.Collections.Generic;

namespace CopyChanges.Services
{
    public interface IFileService
    {
        string ReadFileContent(string fullPath);
        void SaveToFile(string filePath, string data);
        IEnumerable<string> GetAllFiles(string directory);
    }
}
