using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;

namespace Zero.Core.Mvc.Middlewares
{
    public static class NoCacheExtension
    {
        public static void UseResponseNoCache(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                // cache-control: no-store, no-cache, max-age=0
                context.Response.GetTypedHeaders().CacheControl =
                    new CacheControlHeaderValue
                    {
                        NoCache = true,
                        NoStore = true,
                        MaxAge = TimeSpan.FromSeconds(0)
                    };
                // pragma: no-cache
                context.Response.Headers.Add("pragma", "no-cache");
                await next();
            });
        }
    }
}