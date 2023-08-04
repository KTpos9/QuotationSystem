using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Data.Repositories.Interfaces;
using QuotationSystem.Data.Sessions;
using QuotationSystem.Models.StockIn;
using System;
using System.Collections.Generic;

namespace QuotationSystem.Controllers
{
    [AllowAnonymous]
    public class StockInController : Controller
    {
        private readonly IWHRepository wHRepository;
        private readonly IItemRepository itemRepository;
        private readonly ISessionContext sessionContext;
        private readonly IStockRepository stockRepository;


        public StockInController(IWHRepository _wHRepository, IItemRepository _itemRepository, ISessionContext sessionContext, IStockRepository stockRepository)
        {
            this.wHRepository = _wHRepository;
            this.itemRepository = _itemRepository;
            this.sessionContext = sessionContext;
            this.stockRepository = stockRepository;
        }

        public IActionResult Index()
        {
            List<string> whId = wHRepository.GetAllWHIds();
            var model = new StockInViewModel
            {
                WhIds = whId
            };
            return View(model);
        }

        public JsonResult GetItemDetailById(string itemCode) => Json(itemRepository.GetItemById(itemCode));

        [HttpPost]
        public IActionResult StockInSubmit(StockInViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Save the stock in data to the database implement this logic here
                var stockIn = new TStock
                {
                    LabelId = model.LabelId,
                    ItemCode = model.ItemCode,
                    Qty = model.Qty,
                    LotNo = model.LotNo,
                    LocationId = model.LocationId,
                    WhId = model.WhId,
                    CreateBy = sessionContext.CurrentUser.Id,
                    StockInDate = DateTime.Now,
                };
                if (stockRepository.addStockIn(stockIn))
                {
                    return Json(new { success = true, message = "Stock in data has been saved successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Stock in data has already been saved" });
                }

            }

            return Json(new { success = false, message = "Stock in data has been saved successfully!" });
        }

    }
}
