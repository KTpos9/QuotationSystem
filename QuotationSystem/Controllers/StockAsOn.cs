using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Models;
using QuotationSystem.Data;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Data.Repositories.Interfaces;
using QuotationSystem.Models.StockAsOn;
using QuotationSystem.Models.StockIn;
using System.Collections.Generic;
using System.Linq;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;
using Microsoft.EntityFrameworkCore;
using Zero.Extension;

namespace QuotationSystem.Controllers
{
    public class StockAsOn : Controller
    {
        private readonly IWHRepository wHRepository;
        private readonly IStockRepository stockRepository;

        public StockAsOn(IWHRepository wHRepository, IStockRepository stockRepository)
        {
            this.wHRepository = wHRepository;
            this.stockRepository = stockRepository;
        }

        public IActionResult Index()
        {
            List<string> whId = wHRepository.GetAllWHIds();
            whId.Insert(0, "");
            var model = new StockAsOnViewModel
            {
                WhIds = whId
            };
            return View(model);
        }
        [HttpPost]
        public JsonResult Search(string itemCode, string whId, DataTableOptionModel option)
        {
            var result = stockRepository.GetStockList(option, itemCode: itemCode, whId: whId);
            return result.ToJsonResult(option);
        }
    }
}
