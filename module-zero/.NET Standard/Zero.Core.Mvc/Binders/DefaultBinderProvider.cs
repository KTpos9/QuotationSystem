using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Zero.Core.Mvc.Binders
{
    public class DefaultBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var dateTimeFormats = new string[] { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm", "MMM/yyyy", "MM/yyyy" };
            var timeSpanFormats = new string[] { @"dd\.hh\:mm", @"d\.hh\:mm", @"hh\:mm", @"hh\:mm\:ss" };

            if (context.Metadata.ModelType == typeof(DateTime))
            {
                return new DateTimeBinder(default(DateTime), dateTimeFormats);
            }

            if (context.Metadata.ModelType == typeof(DateTime?))
            {
                return new DateTimeBinder(default(DateTime?), dateTimeFormats);
            }

            if (context.Metadata.ModelType == typeof(TimeSpan))
            {
                return new TimeSpanBinder(default(TimeSpan), timeSpanFormats);
            }

            if (context.Metadata.ModelType == typeof(TimeSpan?))
            {
                return new TimeSpanBinder(default(TimeSpan?), timeSpanFormats);
            }

            return null;
        }
    }
}