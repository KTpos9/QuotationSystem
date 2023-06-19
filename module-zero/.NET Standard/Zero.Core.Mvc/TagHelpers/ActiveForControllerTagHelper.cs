using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zero.Core.Mvc.TagHelpers
{
    [HtmlTargetElement(Attributes = "active-for-controller")]
    public class ActiveForControllerTagHelper : TagHelper
    {
        public string ActiveForController { get; set; }

        [HtmlAttributeName("active-for-action")]
        public string ActiveForAction { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            List<string> targetControllers = SplitText(ActiveForController);
            List<string> targetActions = SplitText(ActiveForAction);
            if (targetControllers.Any() == false)
            {
                return;
            }

            var routeData = ViewContext.RouteData.Values;
            var activeController = routeData["controller"];
            var activeAction = routeData["action"];

            if (targetControllers.Contains(activeController) && targetActions.Any() == false)
            {
                SetActiveClass(output);
            }
            else if (targetControllers.Contains(activeController) && targetActions.Contains(activeAction))
            {
                SetActiveClass(output);
            }
        }

        private static void SetActiveClass(TagHelperOutput output)
        {
            var attributesClass = output.Attributes["class"];
            if (attributesClass != null)
            {
                output.Attributes.Remove(attributesClass);
                output.Attributes.Add("class", $"{attributesClass.Value} active");
            }
            else
            {
                output.Attributes.Add("class", "active");
            }
        }

        private List<string> SplitText(string value)
        {
            return string.IsNullOrEmpty(value) ?
                new List<string>() :
                value.Split(",").ToList();
        }
    }
}