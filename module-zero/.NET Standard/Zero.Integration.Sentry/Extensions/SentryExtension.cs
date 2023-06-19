using Microsoft.Extensions.DependencyInjection;
using System;
using Zero.Integration.Sentry.Models;
using Zero.Integration.Sentry.Repositories;

namespace Zero.Integration.Sentry.Extensions
{
    public static class SentryExtension
    {
        public static void AddSentry(this IServiceCollection services,
            Func<IServiceProvider, SentryOption> getSentryOption)
        {
            services.AddSingleton(getSentryOption);
            services.AddScoped<ISentryRepository, SentryRepository>();
        }

        public static void AddSentry<TSentryUserContext>(this IServiceCollection services,
            Func<IServiceProvider, SentryOption> getSentryOption)
            where TSentryUserContext : class, ISentryUserContext
        {
            services.AddSingleton(getSentryOption);
            services.AddScoped<ISentryRepository, SentryRepository>();
            services.AddTransient<ISentryUserContext, TSentryUserContext>();    // optional use for user info
        }
    }
}