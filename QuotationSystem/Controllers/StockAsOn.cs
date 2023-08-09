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
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using System.IO;

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
            var result = stockRepository.GetStockList(itemCode: itemCode, whId: whId);
            var response = result.ToList().AsQueryable().ToDataTableResult(option).ToJsonResult(option);
            return response;
        }
        
        public IActionResult ExportToExcel(string itemCode, string whId)
        {
            var result = stockRepository.GetStockList( itemCode: itemCode, whId: whId).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add headers
                worksheet.Cells[1, 1].Value = "Item Code";
                worksheet.Cells[1, 2].Value = "Item Name";
                worksheet.Cells[1, 3].Value = "WH ID";
                worksheet.Cells[1, 4].Value = "Stock Qty";

                using (var headerRange = worksheet.Cells[1, 1, 1, 4])
                {
                    headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                int row = 2;
                foreach (var stocklist in result)
                {
                    worksheet.Cells[row, 1].Value = stocklist.ItemCode;
                    worksheet.Cells[row, 2].Value = stocklist.ItemCodeNavigation.ItemName;
                    worksheet.Cells[row, 3].Value = stocklist.WhId;
                    worksheet.Cells[row, 4].Value = stocklist.Qty;

                    row++;
                }
                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit();
                worksheet.Column(3).AutoFit();
                worksheet.Column(4).AutoFit();
                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockData.xlsx");

            }
        }
        public IActionResult StockAsOnDetail(string itemCode, string whId)
        {
            var model = new StockAsOnDetailViewModel
            {
                ItemCode = itemCode,
                WhId = whId
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetStockAsOnDetail(string itemCode, string whId, DataTableOptionModel option)
        {
            var result = stockRepository.GetLabelList(itemCode: itemCode, whId: whId);
            var x = result.ToList();
            var response = result.ToDataTableResult(option).ToJsonResult(option);
            return response;
        }

    }
}
