using System;
using System.IO.Compression;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Profiling.Storage;
using Zero.System;

namespace Zero.Core.Mvc.Startup
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureSession(this IServiceCollection services,
            string name,
            int idleTimeoutInMinutes)
        {
            return services.AddSession(options =>
            {
                options.Cookie.Name = name;
                options.IdleTimeout = TimeSpan.FromMinutes(idleTimeoutInMinutes);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        public static IServiceCollection ConfigureAntiforgery(this IServiceCollection services, string name)
        {
            return services.AddAntiforgery(options =>
            {
                options.FormFieldName = $"CSRF-TOKEN-{name}";
                options.HeaderName = $"X-CSRF-TOKEN-{name}";
                options.Cookie.Name = $"Cookie.Antiforgery.{name}";
                options.SuppressXFrameOptionsHeader = false;
            });
        }

        public static IServiceCollection ConfigureFormOptions(this IServiceCollection services)
        {
            return services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 10240;
                options.MultipartBodyLengthLimit = 52428800;
            });
        }

        public static IServiceCollection ConfigureResponseCompression(this IServiceCollection services)
        {
            return services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
        }

        public static IServiceCollection ConfigureMiniProfiler(this IServiceCollection services,
            bool enableMiniProfiler,
            int cacheDurationInMinutes)
        {
            if (enableMiniProfiler)
            {
                services.AddMiniProfiler(options =>
                    {
                        options.RouteBasePath = "/profiler";
                        ((MemoryCacheStorage)options.Storage).CacheDuration = TimeSpan.FromMinutes(cacheDurationInMinutes);
                        options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
                    })
                    .AddEntityFramework();
            }

            return services;
        }

        public static IServiceCollection ConfigureSystemTime(this IServiceCollection services,
            bool enable,
            DateTime systemTime)
        {
            if (enable)
            {
                SystemTime.SetDateTime(systemTime);
            }

            return services;
        }
    }
}