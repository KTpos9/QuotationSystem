using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Zero.Core.Mvc.TagHelpers
{
    [HtmlTargetElement(Attributes = "active-for-suffix-controller")]
    public class ActiveForSuffixControllerTagHelper : TagHelper
    {
        public string ActiveForSuffixController { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routeData = ViewContext.RouteData.Values;
            var activeController = routeData["controller"] as string;

            if (activeController.EndsWith(ActiveForSuffixController))
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
    }
}