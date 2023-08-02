using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Data.Repositories.Interfaces;
using QuotationSystem.Models.StockIn;
using System.Collections.Generic;

namespace QuotationSystem.Controllers
{
    [AllowAnonymous]
    public class StockInController : Controller
    {
        private readonly IWHRepository wHRepository;
        private readonly IItemRepository itemRepository;

        
        public StockInController( IWHRepository _wHRepository, IItemRepository _itemRepository)
        {
            this.wHRepository = _wHRepository;
            this.itemRepository = _itemRepository;
        }

        public IActionResult Index()
        {
            List<string> whId = wHRepository.GetAllWHIds();
            var model = new StockInModel
            {
                WhIds = whId
            };
            return View(model);
        }

        public JsonResult GetItemDetailById(string itemCode) => Json(itemRepository.GetItemById(itemCode));
    }
}
