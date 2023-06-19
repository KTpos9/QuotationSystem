using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Models;
using QuotationSystem.Models.Home;
using System.Diagnostics;

namespace QuotationSystem.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuotationRepository quotationRepository;

        public HomeController(ILogger<HomeController> logger, IQuotationRepository quotationRepository)
        {
            _logger = logger;
            this.quotationRepository = quotationRepository;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                QuotationHeader = quotationRepository.GetTodayQuotationHeader(),
                WeeklyCount = quotationRepository.GetWeeklyCount(),
                MonthlyCount = quotationRepository.GetMonthlyCount()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
