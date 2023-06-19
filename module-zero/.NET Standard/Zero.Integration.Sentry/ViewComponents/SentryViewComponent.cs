using Microsoft.AspNetCore.Mvc;
using System;
using Zero.Integration.Sentry.Models;

namespace Zero.Integration.Sentry.ViewComponents
{
    public class SentryViewComponent : ViewComponent
    {
        private readonly SentryOption sentryOption;
        private readonly IServiceProvider serviceProvider;

        public SentryViewComponent(SentryOption sentryOption,
            IServiceProvider serviceProvider)
        {
            this.sentryOption = sentryOption;
            this.serviceProvider = serviceProvider;
        }

        public IViewComponentResult Invoke()
        {
            ISentryUserContext userContext = (ISentryUserContext)serviceProvider.GetService(typeof(ISentryUserContext));
            var model = new SentryViewComponentModel()
            {
                Option = sentryOption,
                UserContext = userContext
            };
            return View(model);
        }
    }

    public class SentryViewComponentModel
    {
        public SentryOption Option { get; set; }
        public ISentryUserContext UserContext { get; set; }
    }
}