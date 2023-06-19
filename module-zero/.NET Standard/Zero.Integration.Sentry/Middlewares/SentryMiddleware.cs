using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zero.Core.Mvc.Extensions;
using Zero.Integration.Sentry.Models;
using Zero.Integration.Sentry.Repositories;

namespace Zero.Integration.Sentry.Middlewares
{
    public class SentryMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public SentryMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
        }

        public async Task Invoke(HttpContext httpContext, ISentryRepository sentryRepository)
        {
            try
            {
                await requestDelegate(httpContext);
            }
            catch (Exception ex)
            {
                ISentryUserContext userContext = GetUserContext(httpContext);
                Dictionary<string, object> extras = GetExtras(httpContext);

                sentryRepository.CaptureException(ex, userContext, extras);

                // We're not handling, just logging. Throw it for someone else to take care of it.
                throw;
            }
        }

        private ISentryUserContext GetUserContext(HttpContext httpContext)
        {
            return httpContext.Resolving<ISentryUserContext>();
        }

        private Dictionary<string, object> GetExtras(HttpContext httpContext)
        {
            var extras = new Dictionary<string, object>();
            extras.Add("Http Method", httpContext.Request.Method);
            extras.Add("Path", httpContext.Request.Path + httpContext.Request.QueryString);
            return extras;
        }
    }
}