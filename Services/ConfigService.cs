using CopyChanges.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CopyChanges.Services
{
    public class ConfigService : IConfigService
    {
        public List<string> LoadConfiguration(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Configuration file not found.");

            var jsonString = File.ReadAllText(filePath);
            var jsonDoc = JsonDocument.Parse(jsonString);
            if (!jsonDoc.RootElement.TryGetProperty("EditorsContent", out var editorsProperty))
                throw new Exception("Invalid configuration file structure.");

            var editorsContent = new List<string>();
            foreach (var element in editorsProperty.EnumerateArray())
            {
                editorsContent.Add(element.GetString() ?? string.Empty);
            }

            return editorsContent;
        }

        public void SaveConfiguration(string filePath, List<string> editorsContent)
        {
            var data = new Dictionary<string, object>
            {
                ["EditorsContent"] = editorsContent
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, jsonString);
        }
    }
}
