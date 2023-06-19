using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Zero.Core.Mvc.Binders
{
    public class TimeSpanBinder : IModelBinder
    {
        private readonly object defaultValue;
        private readonly string[] formats;

        public TimeSpanBinder(object defaultValue, string[] formats)
        {
            this.defaultValue = defaultValue;
            this.formats = formats;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var providerResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (providerResult.Length == 0)
            {
                bindingContext.Result = ModelBindingResult.Success(defaultValue);
                return Task.CompletedTask;
            }

            var isSuccess = TimeSpan.TryParseExact(providerResult.FirstValue,
                formats,
                CultureInfo.InvariantCulture,
                TimeSpanStyles.None,
                out TimeSpan outTime);

            if (isSuccess == false)
            {
                bindingContext.Result = ModelBindingResult.Success(defaultValue);
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(outTime);
            return Task.CompletedTask;
        }
    }
}