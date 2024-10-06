namespace CopyChanges.Constants
{
    public static class PathConstants
    {
        public const string BaseDirectory = "C:\\CompareChanges";
        public const string JsonFilePath = "C:\\Users\\morge\\copy-select\\selections.json";
        public static string File1Path => System.IO.Path.Combine(BaseDirectory, "file1.txt");
        public static string File2Path => System.IO.Path.Combine(BaseDirectory, "file2.txt");
    }

}

