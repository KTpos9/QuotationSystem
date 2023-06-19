using System;
using System.Reflection;

namespace Zero.Core.Razor.RazorEngine
{
    public static class RazorEngineTemplateHelper<TTemplateViewModel>
    {
        private const string dynamicAssemblyNamespace = "TemplateDynamicAssembly";

        public static string MergeViewModelReturnMarkup(TTemplateViewModel templateViewModel, Assembly templateAssembly)
        {
            // the generated type is defined in the namespace that we defined.
            // "Template" is the type name that razor uses by default.

            RazorEngineBaseTemplate<TTemplateViewModel> razorEngineBaseTemplate =
              (RazorEngineBaseTemplate<TTemplateViewModel>)Activator.CreateInstance(templateAssembly.GetType($"{dynamicAssemblyNamespace}.Template"));

            // add the view model [associated with the view] to the template
            razorEngineBaseTemplate.Model = templateViewModel;

            // run the code.
            razorEngineBaseTemplate.ExecuteAsync().Wait();

            return razorEngineBaseTemplate.GetMarkup();
        }
    }
}