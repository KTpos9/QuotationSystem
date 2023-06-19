using Sentry.Protocol;
using System;
using System.Collections.Generic;
using Zero.Integration.Sentry.Models;

namespace Zero.Integration.Sentry.Repositories
{
    public interface ISentryRepository
    {
        void CaptureException(Exception exception);

        void CaptureException(Exception exception, ISentryUserContext userContext, Dictionary<string, object> extras = null);

        void CaptureMessage(string message, SentryLevel level);

        void CaptureMessage(string message, SentryLevel level, ISentryUserContext userContext, Dictionary<string, object> extras = null);
    }
}