using Microsoft.AspNetCore.Mvc;
using MimeTypes.Core;
using System;
using System.IO;
using Zero.Extension;

namespace Zero.Core.Mvc.Extensions
{
    public static class FileExtension
    {
        public static IActionResult PreviewFile(this ControllerBase controller, string filePath, string downloadFileName = null)
        {
            try
            {
                if (File.Exists(filePath) == false)
                {
                    return controller.NotFound();
                }

                if (filePath.IsPdfFile())
                {
                    // preview pdf
                    return controller.PhysicalFile(filePath, "application/pdf");
                }

                var ext = new FileInfo(filePath).Extension;
                var contentType = MimeTypeMap.GetMimeType(ext);
                if (filePath.IsImageFile())
                {
                    // preview image
                    return controller.PhysicalFile(filePath, contentType);
                }

                var downloadName = downloadFileName ?? Path.GetFileName(filePath);
                return controller.PhysicalFile(filePath, contentType, downloadName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public static IActionResult DownloadFile(this ControllerBase controller, string filePath, string downloadFileName = null)
        {
            try
            {
                if (File.Exists(filePath) == false)
                {
                    return controller.NotFound();
                }

                var ext = new FileInfo(filePath).Extension;
                var contentType = MimeTypeMap.GetMimeType(ext);
                var downloadName = downloadFileName ?? Path.GetFileName(filePath);

                return controller.PhysicalFile(filePath, contentType, downloadName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}