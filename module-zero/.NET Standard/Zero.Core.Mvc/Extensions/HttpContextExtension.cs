using Microsoft.AspNetCore.Http;

namespace Zero.Core.Mvc.Extensions
{
    public static class HttpContextExtension
    {
        public static T Resolving<T>(this HttpContext context) where T : class
        {
            return context.RequestServices.GetService(typeof(T)) as T;
        }
    }
}