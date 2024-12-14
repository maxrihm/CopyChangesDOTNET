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
            // Check absolute path first
            if (Path.IsPathRooted(line) && File.Exists(line))
            {
                return true;
            }

            // If there's a project directory, try relative path
            if (!string.IsNullOrEmpty(_projectDirectory))
            {
                var potentialFilePath = Path.Combine(_projectDirectory, line);
                return File.Exists(potentialFilePath);
            }

            return false;
        }

        public override string Handle(string line)
        {
            if (CanHandle(line))
            {
                string fullPath;

                if (Path.IsPathRooted(line))
                {
                    fullPath = line;
                }
                else
                {
                    fullPath = Path.Combine(_projectDirectory, line);
                }

                var content = _fileService.ReadFileContent(fullPath);

                // If we have a project directory, show relative path; otherwise, show full path
                string displayedPath = !string.IsNullOrEmpty(_projectDirectory)
                    ? Path.GetRelativePath(_projectDirectory, fullPath)
                    : fullPath;

                var sb = new StringBuilder();
                sb.AppendLine($"// {displayedPath}");
                sb.AppendLine(content);
                sb.AppendLine();
                return sb.ToString();
            }

            return PassToNext(line);
        }
    }
}
