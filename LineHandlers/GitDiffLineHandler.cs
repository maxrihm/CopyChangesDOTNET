using System;
using System.Diagnostics;
using System.Text;

namespace CopyChanges.LineHandlers
{
    public class GitDiffLineHandler : BaseLineHandler
    {
        private readonly string _projectDirectory;

        public GitDiffLineHandler(string projectDirectory)
        {
            _projectDirectory = projectDirectory;
        }

        public override bool CanHandle(string line)
        {
            return string.Equals(line.Trim(), "CGD", StringComparison.OrdinalIgnoreCase);
        }

        public override string Handle(string line)
        {
            if (CanHandle(line))
            {
                // Just run `git diff` and return the output
                return RunGitDiff();
            }

            return PassToNext(line);
        }

        private string RunGitDiff()
        {
            try
            {
                // This runs the native `git diff` command for the entire working directory
                var startInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "diff",  // Get the diff for all changes in the working directory
                    WorkingDirectory = _projectDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        return output;  // Return the git diff output
                    }
                    else
                    {
                        string error = process.StandardError.ReadToEnd();
                        return $"Error: {error}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Exception occurred while running git diff: {ex.Message}";
            }
        }
    }
}
