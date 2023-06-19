using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Zero.Core.Mvc.Binders
{
    public class DateTimeBinder : IModelBinder
    {
        private readonly object defaultDateTime;
        private readonly string[] binderFormats;

        public DateTimeBinder(object defaultDateTime, string[] binderFormats)
        {
            this.defaultDateTime = defaultDateTime;
            this.binderFormats = binderFormats;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var providerResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (providerResult.Length == 0)
            {
                bindingContext.Result = ModelBindingResult.Success(defaultDateTime);
                return Task.CompletedTask;
            }

            var isSuccess = DateTime.TryParseExact(providerResult.FirstValue,
                binderFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime outTime);

            if (isSuccess == false)
            {
                bindingContext.Result = ModelBindingResult.Success(defaultDateTime);
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(outTime);
            return Task.CompletedTask;
        }
    }
}