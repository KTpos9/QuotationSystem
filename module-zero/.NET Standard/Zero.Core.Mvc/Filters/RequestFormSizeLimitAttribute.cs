using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Zero.Core.Mvc.Filters
{
    /// <summary>
    /// Filter to set size limits for request form data
    /// ref: https://stackoverflow.com/questions/38357108/form-submit-resulting-in-invaliddataexception-form-value-count-limit-1024-exce
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestFormSizeLimitAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
    {
        private readonly FormOptions formOptions;

        public RequestFormSizeLimitAttribute(int valueCountLimit)
        {
            formOptions = new FormOptions
            {
                KeyLengthLimit = valueCountLimit,
                ValueCountLimit = valueCountLimit,
                ValueLengthLimit = valueCountLimit
            };
        }

        public int Order { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var features = context.HttpContext.Features;
            var formFeature = features.Get<IFormFeature>();

            if (formFeature?.Form == null)
            {
                // Request form has not been read yet, so set the limits
                features.Set<IFormFeature>(new FormFeature(context.HttpContext.Request, formOptions));
            }
        }
    }
}