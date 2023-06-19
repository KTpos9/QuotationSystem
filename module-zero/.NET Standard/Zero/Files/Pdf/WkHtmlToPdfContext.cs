using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Zero.Files.Pdf
{
    public class WkHtmlToPdfContext : IWkHtmlToPdfContext
    {
        public FileInfo WkHtmlToPdfFilePath { get; set; }
        public DirectoryInfo TemporaryFileDirectory { get; set; }
        public Dictionary<string, string> DefaultOptions { get; }

        public WkHtmlToPdfContext(string wkHtmlToPdfFilePath, string tempFileDirectory)
        {
            WkHtmlToPdfFilePath = new FileInfo(wkHtmlToPdfFilePath);
            TemporaryFileDirectory = new DirectoryInfo(tempFileDirectory);
            DefaultOptions = new Dictionary<string, string>
            {
                {"--page-size", "A4"},
                {"--dpi", "300"},
                {"--margin-bottom", "10"},
                {"--margin-left", "15"},
                {"--margin-right", "10"},
                {"--margin-top", "15"}
            };
        }

        public WkHtmlToPdfContext(string wkHtmlToPdfFilePath, string tempFileDirectory, Dictionary<string, string> defaultOptions)
        {
            WkHtmlToPdfFilePath = new FileInfo(wkHtmlToPdfFilePath);
            TemporaryFileDirectory = new DirectoryInfo(tempFileDirectory);
            DefaultOptions = defaultOptions;
        }

        public FileInfo RenderUrl(string url, Dictionary<string, string> option = null)
        {
            var pdf = GetTemporaryFilePath("pdf");

            RunProcess(url, pdf, option);

            return pdf;
        }

        public FileInfo RenderContent(string content, Dictionary<string, string> option = null)
        {
            var htmlPath = GetTemporaryFilePath("html");
            try
            {
                File.WriteAllText(htmlPath.FullName, content);
                return RenderFile(htmlPath.FullName, option);
            }
            finally
            {
                File.Delete(htmlPath.FullName);
            }
        }

        public FileInfo RenderFile(string htmlFilePath, Dictionary<string, string> option = null)
        {
            var pdf = GetTemporaryFilePath("pdf");

            RunProcess(htmlFilePath, pdf, option);

            return pdf;
        }

        private FileInfo GetTemporaryFilePath(string filExtension)
        {
            var filePath = Path.Combine(TemporaryFileDirectory.FullName, $"{Guid.NewGuid()}.{filExtension}");
            var fileInfo = new FileInfo(filePath);

            if (Directory.Exists(fileInfo.Directory?.FullName) == false)
            {
                Directory.CreateDirectory(fileInfo.Directory?.FullName);
            }
            if (File.Exists(fileInfo.FullName))
            {
                File.Delete(fileInfo.FullName);
            }

            return fileInfo;
        }

        private void RunProcess(string inputPath, FileInfo outputPath, Dictionary<string, string> option)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = WkHtmlToPdfFilePath.FullName,
                    Arguments = $"{GetOptions(option)}  \"{inputPath}\"  \"{outputPath.FullName}\""
                },
                EnableRaisingEvents = true
            };

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"error: {error}");
            }
        }

        private string GetOptions(Dictionary<string, string> option)
        {
            var optionsKeys = option == null ?
                DefaultOptions.Select(item => $"{item.Key}  {item.Value}").ToList() :
                option.Select(item => $"{item.Key}  {item.Value}").ToList();

            return string.Join("  ", optionsKeys);
        }
    }
}