using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Globalization;

namespace Zero.Core.Mvc.Startup
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseMiniProfiler(this IApplicationBuilder app, bool enabled)
        {
            if (enabled)
            {
                return app.UseMiniProfiler();
            }

            return app;
        }

        public static IApplicationBuilder UseStaticFilesWithCache(this IApplicationBuilder app, string totalSeconds = "2592000")
        {
            return app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = responseContext =>
                {
                    var cacheHeader = $"public,max-age={totalSeconds}";
                    responseContext.Context.Response.Headers[HeaderNames.CacheControl] = cacheHeader;
                }
            });
        }

        public static IApplicationBuilder UseRequestLocalization(this IApplicationBuilder app, CultureInfo culture)
        {
            return app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = new List<CultureInfo> { culture },
                SupportedUICultures = new List<CultureInfo> { culture }
            });
        }
    }
}