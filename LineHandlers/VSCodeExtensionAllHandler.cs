using CopyChanges.Constants;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using CopyChanges.Interfaces;
using System.Text;

namespace CopyChanges.LineHandlers
{
    public class VSCodeExtensionAllHandler : BaseLineHandler
    {
        private readonly IJsonService _jsonService;
        private readonly string _projectDirectory;

        public VSCodeExtensionAllHandler(IJsonService jsonService, string projectDirectory)
        {
            _jsonService = jsonService;
            _projectDirectory = projectDirectory;
        }

        public override bool CanHandle(string line)
        {
            return line.Length == 1 && line == "V";  // Ensure it's exactly "V"
        }

        public override string Handle(string line)
        {
            if (CanHandle(line))
            {
                var jsonData = _jsonService.LoadJsonFile(PathConstants.JsonFilePath);
                var result = ProcessJsonData(jsonData);
                return result;
            }

            return PassToNext(line);
        }

        private string ProcessJsonData(Dictionary<string, object> jsonData)
        {
            var result = new StringBuilder();

            // Group by file paths and combine the blocks that belong to the same file
            var groupedFiles = jsonData
                .Select(entry => new
                {
                    FilePath = entry.Key,
                    Blocks = JsonSerializer.Deserialize<List<ContentBlock>>(entry.Value.ToString())
                })
                .GroupBy(entry => entry.FilePath);

            foreach (var fileGroup in groupedFiles)
            {
                // If we have a project directory, we try to get a relative path.
                // Otherwise, we just use the full path.
                string displayedPath;
                if (!string.IsNullOrEmpty(_projectDirectory))
                {
                    displayedPath = Path.GetRelativePath(_projectDirectory, fileGroup.Key);
                }
                else
                {
                    // No project directory: just show the full path
                    displayedPath = fileGroup.Key;
                }

                result.AppendLine($"Partial code of file {displayedPath}:");

                foreach (var block in fileGroup.SelectMany(g => g.Blocks))
                {
                    result.AppendLine("...");
                    result.AppendLine(block.content); // Correctly append the content from the JSON blocks
                    result.AppendLine("...");
                }
            }

            return result.ToString();
        }

        private class ContentBlock
        {
            public int startLine { get; set; }
            public int endLine { get; set; }
            public string content { get; set; }
        }
    }
}
