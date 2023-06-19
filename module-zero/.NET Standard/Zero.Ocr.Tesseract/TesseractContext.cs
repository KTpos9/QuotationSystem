using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Zero.Ocr.Tesseract
{
    public class TesseractContext : ITesseractContext
    {
        private readonly FileInfo exe = new FileInfo("C:\\Program Files\\Tesseract-OCR\\tesseract.exe");
        private readonly DirectoryInfo temporaryFileDirectory;

        private readonly Dictionary<string, string> defaultOptions = new Dictionary<string, string>
            {
                {"--dpi", "300"}
            };

        public TesseractContext(string tempFileDirectory)
        {
            temporaryFileDirectory = new DirectoryInfo(tempFileDirectory);
        }

        public TesseractContext(string exeFilePath, string tempFileDirectory)
        {
            exe = new FileInfo(exeFilePath);
            temporaryFileDirectory = new DirectoryInfo(tempFileDirectory);
        }

        public string[] ReadText(string imageFilePath, Dictionary<string, string> option = null)
        {
            var tempFile = GetTemporaryFilePath();

            RunProcess(imageFilePath, tempFile, option);

            var output = File.ReadAllLines(tempFile.FullName + ".txt");
            File.Delete(tempFile.FullName + ".txt");
            return output;
        }

        private FileInfo GetTemporaryFilePath()
        {
            var filePath = Path.Combine(temporaryFileDirectory.FullName, $"{Guid.NewGuid()}");
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
                    FileName = exe.FullName,
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
                defaultOptions.Select(item => $"{item.Key}  {item.Value}").ToList() :
                option.Select(item => $"{item.Key}  {item.Value}").ToList();

            return string.Join("  ", optionsKeys);
        }
    }
}