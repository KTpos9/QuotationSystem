using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Data.Sessions;
using QuotationSystem.Models;
using QuotationSystem.Models.Home;
using System.Diagnostics;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;

namespace QuotationSystem.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuotationRepository quotationRepository;
        private readonly ISessionContext sessionContext;

        public HomeController(ILogger<HomeController> logger, IQuotationRepository quotationRepository, ISessionContext sessionContext)
        {
            _logger = logger;
            this.quotationRepository = quotationRepository;
            this.sessionContext = sessionContext;
        }

        public IActionResult Index()
        {
            if (sessionContext.IsLoggedIn)
            {
                (int todayCount, int weeklyCount, int monthlyCount) = quotationRepository.GetQuotationCounts();
                var model = new HomeViewModel
                {
                    TodayCount = todayCount,
                    WeeklyCount = weeklyCount,
                    MonthlyCount = monthlyCount
                };
                return View(model);
            }
            return RedirectToAction("Login", "Account");
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
        public IActionResult GetTodayQuotation(DataTableOptionModel option)
        {
            var result = quotationRepository.GetTodayQuotationHeader(option);
            return result.ToJsonResult(option);
        }
    }
}
