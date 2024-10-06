// LineHandlers/TextLineHandler.cs
using System.IO;

namespace CopyChanges.LineHandlers
{
    public class TextLineHandler : BaseLineHandler
    {
        public override bool CanHandle(string line)
        {
            return !string.IsNullOrEmpty(line) && !File.Exists(line); // Regular text
        }

        public override string Handle(string line)
        {
            if (CanHandle(line))
            {
                return $"{line}";
            }

            return PassToNext(line);
        }
    }
}
