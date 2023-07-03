using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace QuotationSystem.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers = null, string actions = null, string cssClass = null)
        {
            if (string.IsNullOrEmpty(cssClass))
            {
                cssClass = "active";
            }

            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            string currentController = (string)htmlHelper.ViewContext.RouteData.Values["controller"];

            List<string> acceptedActions = (actions ?? currentAction).Split(',').ToList();
            List<string> acceptedControllers = (controllers ?? currentController).Split(',').ToList();

            return acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction) ?
                cssClass : string.Empty;
        }

        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            var currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            return currentAction;
        }
    }
}