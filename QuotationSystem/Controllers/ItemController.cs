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

namespace QuotationSystem.Controllers
{
    public class ItemController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IItemRepository itemRepository;
        public ItemController(IUserRepository userRepository, IItemRepository itemRepository)
        {
            this.userRepository = userRepository;
            this.itemRepository = itemRepository;
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
                Item = item
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
            itemRepository.EditItem(itemModel.Item);
            return RedirectToAction("ItemList");
        }
        [HttpPost]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            await UploadFile(file);
            return RedirectToAction("ItemList");
        }
        private async Task UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length == 0 || file == null)
                {
                    return;
                }
                var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                    var data = ExcelToList<MItem>(worksheet);
                    await itemRepository.AddItemByExcel(data);
                    memoryStream.Dispose();
                }
                //string filename = file.FileName;
                //string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));
                //using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                //{
                //    await file.CopyToAsync(filestream);
                //}
            }
            catch (NullReferenceException)
            {
                throw;
                //return BadRequest("Check file format and try again");
            }
        }
        private List<T> ExcelToList<T>(ExcelWorksheet worksheet)
        {
            List<T> list = new List<T>();

            //get first row(column name)
            var columnInfo = Enumerable.Range(1, worksheet.Dimension.Columns).Select(x =>
                new {Index = x, ColumnName = worksheet.Cells[1,x].Value.ToString() }
            );

            for(int row = 2; row < worksheet.Dimension.Rows; row++)
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                var properties = typeof(T).GetProperties();
                int propertiesNotInExcelColumn = properties.Length - columnInfo.ToList().Count;

                //loop over the properties according to the excel column name count
                //map the value from the excel file to the given type
                for (int i = 0; i < properties.Length - propertiesNotInExcelColumn; i++)
                {
                    int col = columnInfo.SingleOrDefault(c => c.ColumnName == properties[i].Name).Index;
                    var val = worksheet.Cells[row, col].Value;
                    var propertyType = properties[i].PropertyType;
                    properties[i].SetValue(obj, Convert.ChangeType(val, propertyType));
                }

                //foreach (var property in typeof(T).GetProperties())
                //{
                //    int col = columnInfo.SingleOrDefault(c => c.ColumnName == property.Name).Index;
                //    var val = worksheet.Cells[row, col].Value;
                //    var propertyType = property.PropertyType;
                //    property.SetValue(obj, Convert.ChangeType(val, propertyType));
                //}
                list.Add(obj);
            }
            return list;
        }
    }
}
