using Sentry;
using Sentry.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using Zero.Integration.Sentry.Models;

namespace Zero.Integration.Sentry.Repositories
{
    public class SentryRepository : ISentryRepository
    {
        private readonly SentryOption option;

        public SentryRepository(SentryOption option)
        {
            this.option = option;
        }

        public void CaptureException(Exception exception)
        {
            CaptureException(exception, null);
        }

        public void CaptureException(Exception exception, ISentryUserContext userContext, Dictionary<string, object> extras = null)
        {
            if (option.Enabled)
            {
                using (SentrySdk.Init(option.Dsn))
                {
                    ConfigureScope(userContext, extras);
                    SentrySdk.CaptureException(exception);
                }
            }
        }

        public void CaptureMessage(string message, SentryLevel level)
        {
            CaptureMessage(message, level, null);
        }

        public void CaptureMessage(string message, SentryLevel level, ISentryUserContext userContext, Dictionary<string, object> extras = null)
        {
            if (option.Enabled)
            {
                using (SentrySdk.Init(option.Dsn))
                {
                    ConfigureScope(userContext, extras);
                    SentrySdk.CaptureMessage(message, level);
                }
            }
        }

        private void ConfigureScope(ISentryUserContext userContext, Dictionary<string, object> extras)
        {
            SentrySdk.ConfigureScope(scope =>
            {
                if (userContext != null)
                {
                    scope.User = new User
                    {
                        Id = userContext.Id,
                        Username = userContext.Username,
                        Email = userContext.Email
                    };
                }

                if (string.IsNullOrWhiteSpace(option.Environment) == false)
                {
                    scope.Environment = option.Environment;
                }
                
                if (extras != null && extras.Any())
                {
                    foreach (var item in extras)
                    {
                        scope.SetExtra(item.Key, item.Value);
                    }
                }

                if (string.IsNullOrWhiteSpace(option.Release) == false)
                {
                    // workaround on sentry@v2
                    scope.SetExtra("Release", option.Release);
                }
            });
        }
    }
}