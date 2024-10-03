using System.Collections.Generic;

namespace CopyChanges.Services
{
    public interface IJsonService
    {
        Dictionary<string, object> LoadJsonFile(string jsonPath);
        void SaveJsonFile(string jsonPath, Dictionary<string, object> data);
    }
}
