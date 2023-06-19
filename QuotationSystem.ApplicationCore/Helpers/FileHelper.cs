using Microsoft.AspNetCore.Http;
using System.IO;
using Zero.Core.Mvc.Extensions;

namespace QuotationSystem.ApplicationCore.Helpers
{
    public static class FileHelper
    {
        public static void CreateImageFile(this string path, IFormFile file, string fileName, int imageSize)
        {
            if (string.IsNullOrEmpty(fileName) == false)
            {
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }

                var savefilePath = Path.Combine(path, fileName);
                file.ScaleAndSaveImageAs(savefilePath, imageSize);
            }
        }

        public static void DeleteFile(this string path, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) == false)
            {
                string fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }
    }
}