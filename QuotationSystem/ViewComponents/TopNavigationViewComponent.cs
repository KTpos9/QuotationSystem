using Microsoft.AspNetCore.Mvc;

namespace WebApp.ViewComponents
{
    public class TopNavigationViewComponent : ViewComponent
    {
        public TopNavigationViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}