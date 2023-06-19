using Microsoft.AspNetCore.Builder;

namespace Zero.Integration.Sentry.Middlewares
{
    public static class SentryExtension
    {
        /// <summary>
        /// Should add after app.UseSession and app.UseDeveloperExceptionPage
        /// </summary>
        /// <param name="app"></param>
        public static void UseSentry(this IApplicationBuilder app)
        {
            // SentryMiddleWare should be after UseSession and UseDeveloperExceptionPage
            app.UseMiddleware<SentryMiddleware>();
        }
    }
}