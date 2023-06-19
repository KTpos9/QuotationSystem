using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.IO;
using Zero.Core.Mvc.Extensions;

namespace QuotationSystem.Data.Configurations
{
    public class ConfigurationContext : IConfigurationContext
    {
        private readonly IWebHostEnvironment env;

        public ConfigurationContext(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public bool IsDevelopment()
        {
            return env.IsDevelopment();
        }

        public CultureInfo SiteLocale => new CultureInfo(Configuration["SiteLocale"]);

        public int SessionTimeout => Convert.ToInt32(Configuration["SessionTimeout"]);

        public FileInfo WkHtmlPath()
        {
            var wkhtmlToPdfPath = Configuration.GetValue<string>("WkHtml:WkHtmlPath");
            if (string.IsNullOrEmpty(wkhtmlToPdfPath) || File.Exists(wkhtmlToPdfPath) == false)
            {
                throw new FileNotFoundException(wkhtmlToPdfPath);
            }
            return new FileInfo(wkhtmlToPdfPath);
        }

        public string WebRootPath => env.WebRootPath;

        public string UploadEmployeeImagePath => Configuration["UploadEmployeeImage"];

        public int ImageSizeInPx => Convert.ToInt32(Configuration["ImageSizeInPx"]);

        public string[] SystemAdmins => Configuration["SystemAdmins"].Split(',');
    }
}