using Microsoft.AspNetCore.Http;
using System.IO;
using Zero.Extension;
using Zero.System.Drawing;

namespace Zero.Core.Mvc.Extensions
{
    public static class FormFileExtension
    {
        public static bool IsValid(this IFormFile formFile)
        {
            return formFile != null && formFile.Length > 0;
        }

        public static bool IsValidFileSize(this IFormFile formFile, long sizeInBytes)
        {
            return IsValid(formFile) && formFile.Length <= sizeInBytes;
        }

        public static bool IsImage(this IFormFile formFile)
        {
            return formFile.FileName.IsImageFile();
        }

        public static bool IsDocument(this IFormFile formFile)
        {
            return formFile.FileName.IsDocumentFile();
        }

        public static string ReadToEnd(this IFormFile formFile)
        {
            if (IsValid(formFile) == false)
            {
                return string.Empty;
            }

            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public static void SaveAs(this IFormFile formFile, string filePath)
        {
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }
        }

        public static void ScaleAndSaveImageAs(this IFormFile formFile, string filePath, int scaleDownImageWidthAndHeightTo)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                var bytes = ms.ToArray();

                new ImageResizer().ScaleAndSaveImage(bytes, scaleDownImageWidthAndHeightTo, filePath);
            }
        }
    }
}