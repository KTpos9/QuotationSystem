using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;

namespace Zero.Core.Mvc.Filters
{
    public class DeleteDownloadedFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Result is PhysicalFileResult fileResult && File.Exists(fileResult.FileName))
            {
                File.Delete(fileResult.FileName);
            }
        }
    }
}