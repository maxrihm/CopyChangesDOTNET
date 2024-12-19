using System.Collections.Generic;

namespace CopyChanges.Interfaces
{
    public interface IConfigService
    {
        List<string> LoadConfiguration(string filePath);
        void SaveConfiguration(string filePath, List<string> editorsContent);
    }
}
