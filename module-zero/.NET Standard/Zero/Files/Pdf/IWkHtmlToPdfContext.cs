using System.Collections.Generic;
using System.IO;

namespace Zero.Files.Pdf
{
    /// <summary>
    /// To create a pdf file using WkHtmlToPdf library
    /// </summary>
    public interface IWkHtmlToPdfContext
    {
        FileInfo WkHtmlToPdfFilePath { get; set; }
        DirectoryInfo TemporaryFileDirectory { get; set; }
        Dictionary<string, string> DefaultOptions { get; }

        FileInfo RenderContent(string content, Dictionary<string, string> option = null);

        FileInfo RenderFile(string htmlFilePath, Dictionary<string, string> option = null);

        FileInfo RenderUrl(string url, Dictionary<string, string> option = null);
    }
}