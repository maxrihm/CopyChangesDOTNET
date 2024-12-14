namespace CopyChanges.LineHandlers
{
    /// <summary>
    /// A no-op line handler that simply returns the input line as-is.
    /// Used when no project directory is selected, ensuring that copy operations do not fail.
    /// </summary>
    public class NullLineHandler : BaseLineHandler
    {
        public override bool CanHandle(string line)
        {
            // Always handle since this is a fallback
            return true;
        }

        public override string Handle(string line)
        {
            // Just return the line as-is
            return line;
        }
    }
}
