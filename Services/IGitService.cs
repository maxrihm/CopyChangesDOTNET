// Services/IGitService.cs
using System.Collections.Generic;

namespace CopyChanges.Services
{
    public interface IGitService
    {
        IEnumerable<string> GetGitChanges(string projectDirectory);
    }
}
