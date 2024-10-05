using System.Collections.Generic;

namespace CopyChanges.Services
{
    public interface IFileService
    {
        string ReadFileContent(string fullPath);
        IEnumerable<string> GetAllFiles(string directory);
        void WriteFileContent(string fullPath, string content);
    }
}
