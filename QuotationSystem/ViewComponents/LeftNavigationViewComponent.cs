using Microsoft.AspNetCore.Mvc;

namespace WebApp.ViewComponents
{
    public class LeftNavigationViewComponent : ViewComponent
    {
        //private readonly ISessionContext sessionContext;

        public LeftNavigationViewComponent(/*ISessionContext sessionContext*/)
        {
            //this.sessionContext = sessionContext;
        }

        public IViewComponentResult Invoke()
        {
            //var model = new LeftNavigationViewModel
            //{
            //    //Name = sessionContext.CurrentUser.Name
            //    Name = "test"
            //};
            //return View(model);
            return View();
        }
    }
}