using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace Zero.Core.Mvc.Extensions
{
    public static class RoutingHttpContextExtension
    {
        public static RouteData GetRouteData(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var routingFeature = httpContext.Features[typeof(IRoutingFeature)] as IRoutingFeature;
            return routingFeature?.RouteData;
        }

        public static RouteValueDictionary GetRouteValues(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var routingFeature = httpContext.Features[typeof(IRoutingFeature)] as IRoutingFeature;
            return routingFeature?.RouteData.Values;
        }

        public static object GetRouteValue(this HttpContext httpContext, string key)
        {
            var values = GetRouteValues(httpContext);

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return values[key];
        }
    }
}