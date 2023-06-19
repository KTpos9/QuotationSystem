using System.IO;

namespace Zero.Extension
{
    public static class LinuxExtension
    {
        public static string ConvertSeparatorChar(this string filePath)
        {
            return filePath.Replace('\\', Path.DirectorySeparatorChar);
        }

        public static string CombinePath(this string path1, string path2, bool replaceSeparator)
        {
            var result = Path.Combine(path1, path2);
            if (replaceSeparator)
            {
                result = result.ConvertSeparatorChar();
            }
            return result;
        }
    }
}