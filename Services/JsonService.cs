using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CopyChanges.Interfaces;

namespace CopyChanges.Services
{
    public class JsonService : IJsonService
    {
        public Dictionary<string, object> LoadJsonFile(string jsonPath)
        {
            try
            {
                var jsonString = File.ReadAllText(jsonPath);
                return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading JSON: {ex.Message}");
            }
        }

        public void SaveJsonFile(string jsonPath, Dictionary<string, object> data)
        {
            var jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText(jsonPath, jsonString);
        }
    }
}