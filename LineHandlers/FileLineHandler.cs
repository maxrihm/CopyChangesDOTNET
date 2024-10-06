using CopyChanges.Interfaces;
using System.IO;
using System.Text;

namespace CopyChanges.LineHandlers
{
    public class FileLineHandler : BaseLineHandler
    {
        private readonly IFileService _fileService;
        private readonly string _projectDirectory;

        public FileLineHandler(IFileService fileService, string projectDirectory)
        {
            _fileService = fileService;
            _projectDirectory = projectDirectory;
        }

        public override bool CanHandle(string line)
        {
            // Check if the line is a valid file path relative to the project directory.
            var potentialFilePath = Path.Combine(_projectDirectory, line);
            
            // DEBUG: Add a breakpoint or log here to ensure it's checking the right path.
            return File.Exists(potentialFilePath);
        }

        public override string Handle(string line)
        {
            if (CanHandle(line))
            {
                // Combine the project directory with the file name to get the full path.
                var fullPath = Path.Combine(_projectDirectory, line);
                var content = _fileService.ReadFileContent(fullPath);
                
                // DEBUG: Add a breakpoint or log to check if this is reached.
                return $"// {Path.GetRelativePath(_projectDirectory, fullPath)}\n{content}\n";
            }

            return PassToNext(line);
        }

    }
}
