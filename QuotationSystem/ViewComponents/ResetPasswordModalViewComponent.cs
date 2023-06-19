using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Repositories;

namespace QuotationSystem.ViewComponents
{
    public class ResetPasswordModalViewComponent : ViewComponent
    {
        private readonly IUserRepository userRepository;
        public ResetPasswordModalViewComponent(IUserRepository userRepository = null)
        {
            this.userRepository = userRepository;
        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
