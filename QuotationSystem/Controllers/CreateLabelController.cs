using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Models.CreateLabel;
using QuotationSystem.Models.User;
using Zero.Security;

namespace QuotationSystem.Controllers
{
    public class CreateLabelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GenerateLabel(CreateLabelModel model)
        {

            return View(model);
        }


    }
}
