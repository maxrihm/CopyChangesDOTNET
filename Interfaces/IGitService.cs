// Services/IGitService.cs
using System.Collections.Generic;

namespace CopyChanges.Interfaces
{
    public interface IGitService
    {
        IEnumerable<string> GetGitChanges(string projectDirectory);
    }
}
