using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Text.RegularExpressions;
using Zero.Web.UserAgentParsers;

namespace Zero.Core.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BrowserFilterAttribute : TypeFilterAttribute
    {
        public BrowserFilterAttribute(string unsupportedBrowserPattern, string controller = null, string action = null) : base(typeof(BrowserFilter))
        {
            Arguments = new object[]
            {
                unsupportedBrowserPattern,
                controller,
                action
            };
        }
    }

    public class BrowserFilter : IAuthorizationFilter
    {
        private readonly string unsupportedBrowserPattern;
        private readonly string controller;
        private readonly string action;

        public BrowserFilter(string unsupportedBrowserPattern, string controller = null, string action = null)
        {
            this.unsupportedBrowserPattern = unsupportedBrowserPattern;
            this.controller = controller ?? "Error";
            this.action = action ?? "Index";
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var ua = context.HttpContext.Request.Headers["User-Agent"].ToString();
            var parser = new UserAgentParser();
            var userAgent = parser.Parse(ua);
            var browser = userAgent.ToString();
            var regex = new Regex(unsupportedBrowserPattern);
            var match = regex.Match(browser);
            if (match.Success)
            {
                context.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                            { "Controller", controller },
                                            { "Action", action }
                                    });
            }
        }
    }
}