using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zero.Core.Mvc.ViewLocators
{
    public class NamespaceViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var subNamespace = GetSubNamespace(context);
            if (string.IsNullOrWhiteSpace(subNamespace) == false)
            {
                string[] locations = { "/Views/" + subNamespace + "/{1}/{0}.cshtml" };
                return locations.Union(viewLocations);  //Add mvc default locations after ours
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["customviewlocation"] = nameof(NamespaceViewLocationExpander);
        }

        private string GetSubNamespace(ViewLocationExpanderContext context)
        {
            var controllerActionDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
                var nameSpace = controllerTypeInfo.AsType().Namespace;
                var subNameSpaces = nameSpace?
                    .Substring(nameSpace.IndexOf(".Controllers", StringComparison.Ordinal) + 1)
                    .Split('.');
                return subNameSpaces?.Length > 1 ? subNameSpaces[1] : "";
            }

            return null;
        }
    }
}