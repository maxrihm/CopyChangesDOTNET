// LineHandlers/FileLineHandler.cs
using CopyChanges.Services;
using System.IO;
using System.Text;

namespace CopyChanges.LineHandlers
{
    public class FileLineHandler : BaseLineHandler
    {
        private readonly IFileService _fileService;

        public FileLineHandler(IFileService fileService)
        {
            _fileService = fileService;
        }

        public override bool CanHandle(string line)
        {
            return File.Exists(line); // Checks if the line is a file path.
        }

        public override string Handle(string line)
        {
            if (CanHandle(line))
            {
                var content = _fileService.ReadFileContent(line);
                return $"{Path.GetFileName(line)}:\n{content}\n-----------------------\n";
            }

            return PassToNext(line);
        }
    }
}
