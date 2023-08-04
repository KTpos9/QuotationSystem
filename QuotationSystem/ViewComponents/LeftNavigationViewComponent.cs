using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Sessions;
using QuotationSystem.Models.ViewComponents;

namespace WebApp.ViewComponents
{
    public class LeftNavigationViewComponent : ViewComponent
    {
        private readonly ISessionContext sessionContext;

        public LeftNavigationViewComponent(ISessionContext sessionContext)
        {
            this.sessionContext = sessionContext;
        }

        public IViewComponentResult Invoke()
        {
            var model = new LeftNavigationViewModel
            {
                Permissions = sessionContext.CurrentUser.RoleIds,
                UserId = sessionContext.CurrentUser.Id
            };
            return View(model);
        }
    }
}