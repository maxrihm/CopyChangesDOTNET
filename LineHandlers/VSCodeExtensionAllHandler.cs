using CopyChanges.Services;
using CopyChanges.Constants;
using System.Collections.Generic;
using System.Text.Json;

namespace CopyChanges.LineHandlers
{
    public class VSCodeExtensionAllHandler : BaseLineHandler
    {
        private readonly IJsonService _jsonService;

        public VSCodeExtensionAllHandler(IJsonService jsonService)
        {
            _jsonService = jsonService;
        }

        public override bool CanHandle(string line)
        {
            return line.Length == 1 && line == "V";  // Check if line is exactly one character and that character is 'V'
        }

        public override string Handle(string line)
        {
            if (CanHandle(line))
            {
                var jsonData = _jsonService.LoadJsonFile(PathConstants.JsonFilePath);  // Use constant for JSON file path
                var result = ProcessJsonData(jsonData);
                return result;
            }

            return PassToNext(line);
        }

        private string ProcessJsonData(Dictionary<string, object> jsonData)
        {
            var result = new System.Text.StringBuilder();

            foreach (var fileEntry in jsonData)
            {
                var filePath = fileEntry.Key;
                var contentBlocks = JsonSerializer.Deserialize<List<ContentBlock>>(fileEntry.Value.ToString());

                foreach (var block in contentBlocks)
                {
                    result.AppendLine($"Partial code of file {filePath}:");
                    result.AppendLine("...");
                    result.AppendLine(block.Content);
                    result.AppendLine("...");
                }
            }

            return result.ToString();
        }

        private class ContentBlock
        {
            public int StartLine { get; set; }
            public int EndLine { get; set; }
            public string Content { get; set; }
        }
    }
}
