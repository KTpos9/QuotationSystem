using System.Threading.Tasks;

namespace Zero.Core.Mvc.View
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}