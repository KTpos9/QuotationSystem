using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Zero.Core.Mvc.Extensions
{
    public static class ModelStateExtension
    {
        public static string GetErrorMessage(this ModelStateDictionary modelState, string separator)
        {
            var message = "";
            if (modelState.IsValid == false)
            {
                var errorMessages = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => x.ErrorMessage))
                    .ToList();

                message = string.Join(separator, errorMessages);
            }
            return message;
        }
    }
}