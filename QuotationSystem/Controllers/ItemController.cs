using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Models.Item;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using QuotationSystem.Data.Models;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;
using System.Text;
using System.ComponentModel;
using QuotationSystem.Data.Helpers;
using QuotationSystem.Helpers;
using System.Data.SqlClient;
using QuotationSystem.Data.Sessions;

namespace QuotationSystem.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository itemRepository;
        private readonly IUnitRepository unitRepository;

        private static string CurrentUser;
        public ItemController(IItemRepository itemRepository, IUnitRepository unitRepository, ISessionContext sessionContext)
        {
            this.itemRepository = itemRepository;
            this.unitRepository = unitRepository;
            CurrentUser = sessionContext.CurrentUser.Id;
        }
        public IActionResult ItemList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Search(string itemCode, string itemName, DataTableOptionModel option)
        {
            var result = itemRepository.GetItemList(option, itemCode: itemCode, itemName: itemName);
            var response = result.ToJsonResult(option);
            return response;
        }
        public PartialViewResult GetEditItemModal(string itemCode)
        {
            var item = itemRepository.GetItemById(itemCode);
            var model = new ItemViewModel
            {
                Item = item,
                UnitLists = unitRepository.GetAllUnitIds()
            };
            return PartialView("_EditItemPartial", model);
        }
        public PartialViewResult GetDeleteItemModal(string itemCode)
        {
            var model = new ItemViewModel
            {
                ItemCode = itemCode
            };
            return PartialView("_DeleteItemPartial", model);
        }
        public IActionResult EditItem(ItemViewModel itemModel)
        {
            itemRepository.EditItem(itemModel.Item, CurrentUser);
            return RedirectToAction("ItemList");
        }
        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            try
            {
                var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                    var excelHelper = new ExcelItemValidation(worksheet, new StringBuilder());
                    var result = excelHelper.ExcelToItemList();

                    if (!excelHelper.IsValidFormat)
                    {
                        memoryStream.Dispose();
                        return File(Encoding.UTF8.GetBytes(excelHelper.ErrorLog.ToString()), "text/plain", "ErrorLog.txt");
                    }
                    await itemRepository.AddItemByExcel(result.ToList());
                    memoryStream.Dispose();
                }
                return RedirectToAction("ItemList");
            }
            catch (NullReferenceException)
            {
                throw;
                //return BadRequest("Check file format and try again");
            }
        }
        [HttpDelete]
        public IActionResult DeleteItem(string itemCode)
        {
            try
            {
                itemRepository.DeleteItem(itemCode);
                return RedirectToAction("ItemList", "Item");
            }
            catch(SqlException ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
