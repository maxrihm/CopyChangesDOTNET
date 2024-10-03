using LibGit2Sharp;
using System.Collections.Generic;
using System.Linq;

namespace CopyChanges.Services
{
    public class GitService : IGitService
    {
        public IEnumerable<string> GetGitChanges(string projectDirectory)
        {
            using var repo = new Repository(projectDirectory);
            var changes = repo.Diff.Compare<TreeChanges>();
            return changes.Select(change => change.Path);
        }
    }
}
