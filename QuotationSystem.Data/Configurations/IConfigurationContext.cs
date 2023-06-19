using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.IO;

namespace QuotationSystem.Data.Configurations
{
    public interface IConfigurationContext
    {
        IConfiguration Configuration { get; }
        int ImageSizeInPx { get; }
        int SessionTimeout { get; }
        CultureInfo SiteLocale { get; }
        string[] SystemAdmins { get; }
        string UploadEmployeeImagePath { get; }
        string WebRootPath { get; }

        bool IsDevelopment();
        FileInfo WkHtmlPath();
    }
}