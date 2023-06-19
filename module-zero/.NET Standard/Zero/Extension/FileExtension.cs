using System.IO;
using System.Linq;
using System.Reflection;
using Zero.Constants;

namespace Zero.Extension
{
    public static class FileExtension
    {
        public static bool IsImageFile(this string filePath)
        {
            return IsValidFileExtension(filePath, FileExtensions.Image);
        }

        public static bool IsPdfFile(this string filePath)
        {
            return IsValidFileExtension(filePath, FileExtensions.Pdf);
        }

        public static bool IsDocumentFile(this string filePath)
        {
            return IsValidFileExtension(filePath, FileExtensions.Documents.ToArray());
        }

        public static bool IsValidFileExtension(string filePath, string[] validFileExtensions)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return false;
            }

            var ext = Path.GetExtension(filePath).ToLower();
            if (validFileExtensions.Contains(ext))
            {
                return true;
            }

            return false;
        }

        public static FileInfo GetInfo(this Assembly assembly)
        {
            FileInfo fi = new FileInfo(assembly.Location);
            return fi;
        }

        public static string GetLastModified(this Assembly assembly)
        {
            FileInfo fi = new FileInfo(assembly.Location);
            return fi.LastWriteTime.ToString("yyyyMMdd_HHmm");
        }
    }
}