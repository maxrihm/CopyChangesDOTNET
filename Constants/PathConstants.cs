namespace CopyChanges.Constants
{
    public static class PathConstants
    {
        public const string BaseDirectory = "C:\\CompareChanges";
        public static string File1Path => System.IO.Path.Combine(BaseDirectory, "file1.txt");
        public static string File2Path => System.IO.Path.Combine(BaseDirectory, "file2.txt");
    }
}

