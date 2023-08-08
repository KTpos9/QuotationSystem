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
            if (itemCode == null)
            {
                itemCode = string.Empty;
            }

            if (whId == null)
            {
                whId = string.Empty;
            }

            var result = stockRepository.GetStockList(option, itemCode: itemCode, whId: whId);
            var response = result.ToList().AsQueryable().ToDataTableResult(option).ToJsonResult(option);
            return response;
        }



    }
}
