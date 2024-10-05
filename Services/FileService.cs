// Services/FileService.cs
using System;
using System.Collections.Generic;
using System.IO;

namespace CopyChanges.Services
{
    public class FileService : IFileService
    {
        public string ReadFileContent(string fullPath)
        {
            try
            {
                return File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                return $"Failed to read {fullPath}: {ex.Message}";
            }
        }

        public IEnumerable<string> GetAllFiles(string directory)
        {
            return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
        }
    }
}