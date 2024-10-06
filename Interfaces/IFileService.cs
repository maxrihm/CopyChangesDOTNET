using System.Collections.Generic;

namespace CopyChanges.Interfaces
{
    public interface IFileService
    {
        string ReadFileContent(string fullPath);
        IEnumerable<string> GetAllFiles(string directory);
        void WriteFileContent(string fullPath, string content);
        bool IsFileIgnored(string filePath, IEnumerable<string> patterns);

    }
}
