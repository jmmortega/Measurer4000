namespace Measurer4000.Core.Utils
{
    public static class ExtensionsFile
    {
        public static bool IsUserInterface(this string file)
        {
            return file.Contains(".axml") || file.Contains(".designer") || file.Contains(".xaml") || file.Contains(".xib");
        }
    }
}
