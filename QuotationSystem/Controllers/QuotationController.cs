using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuotationSystem.ApplicationCore.Constants;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Data.Sessions;
using QuotationSystem.Models.Quotation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;

namespace QuotationSystem.Controllers
{
    //[Authorize(Policy = Policy.UserManagement)]
    [AllowAnonymous]
    public class QuotationController : Controller
    {
        private readonly IQuotationRepository quotationRepository;
        private readonly IConfigRepository configRepository;
        private readonly IItemRepository itemRepository;

        private static string CurrentUser;
        public QuotationController(IQuotationRepository quotationRepository, IConfigRepository configRepository, IItemRepository itemRepository, ISessionContext sessionContext)
        {
            this.quotationRepository = quotationRepository;
            this.configRepository = configRepository;
            this.itemRepository = itemRepository;
            CurrentUser = sessionContext.CurrentUser.Id;
        }
        public IActionResult QuotationList()
        {
            //data for AddQuotationPartial
            var vat = configRepository.GetConfigById("C002");
            var model = new QuotationViewModel
            {
                QuotationNo = GenerateQuotationNo(),
                Date = DateTime.Today,
                Vat = double.Parse(vat)
            };
            var test = quotationRepository.GetQuotationById("QT230700011");
            return View(model);
        }
        public IActionResult PreviewQuotation(string quotationNo)
        {
            var model = new PreviewQuotationViewModel
            {
                Quotation = quotationRepository.GetQuotationById(quotationNo),
                CompanyAddress = configRepository.GetConfigById("C004"),
                CompanyContact = configRepository.GetConfigById("C005"),
                CompanyLogo = configRepository.GetConfigById("C008"),
                CompanyTaxId = configRepository.GetConfigById("C006")
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult AddQuotation(QuotationViewModel model, List<QuotationItemViewModel> itemList)
        {
            try
            {
                double sumOfItem = itemList.Sum(item => double.Parse(item.unitPrice) - (double.Parse(item.unitPrice) * item.discount));
                var quotationHeader = new TQuotationHeader
                {
                    QuotationNo = model.QuotationNo,
                    QuotationDate = model.Date,
                    CustomerName = model.Customer,
                    CustomerAddress = model.CustomerAddress,
                    CustomerContact = model.CustomerContact,
                    TaxId = model.TaxId,
                    Seller = model.SalesName,
                    Vat = model.Vat,
                    CreateBy = CurrentUser,
                    UpdateBy = CurrentUser,
                    ActiveStatus = model.ActiveStatus switch
                    {
                        "on" => "Y",
                        _ => "N"
                    },
                    Total = sumOfItem,
                    GrandTotal = sumOfItem + (sumOfItem * model.Vat),
                    TQuotationDetails = itemList.Select(item => new TQuotationDetail
                    {
                        ItemCode = item.itemCode,
                        ItemQty = item.Qty,
                        DiscountPercent = item.discount / 100,
                        CreateBy = CurrentUser,
                        UpdateBy = CurrentUser
                    }).ToList()
                };
                quotationRepository.AddQuotation(quotationHeader);
                return Ok(new { isSuccess = true });
            }
            catch (SqlException)
            {
                return StatusCode(500, new { isSuccess = false });
            }
        }
        public JsonResult Search(string quotationNo, string customer, DataTableOptionModel option)
        {
            var result = quotationRepository.GetQuotationList(option, qutoationNo: quotationNo, customer: customer);
            var response = result.ToJsonResult(option);
            return response;
        }
        public PartialViewResult GetEditQuotationModal(string itemCode, string currentUser)
        {
            var quotation = quotationRepository.GetQuotationById(itemCode);
            var model = new QuotationViewModel
            {
                QuotationHeader = quotation
            };
            return PartialView("_EditQuotationPartial", model);
        }
        public PartialViewResult GetDeleteQuotationModal(string quotationNo)
        {
            var model = new QuotationViewModel
            {
                QuotationNo = quotationNo
            };
            return PartialView("_DeleteQuotationPartial", model);
        }
        public IActionResult EditQuotation(QuotationViewModel quotationModel)
        {
            //quotationRepository.EditQuotation(quotationModel.QuotationHeader);
            return RedirectToAction("QuotationList");
        }
        private string GenerateQuotationNo()
        {
            var currentYear = DateTime.Now.ToString("yy");
            int currentMonth = DateTime.Now.Month;

            ReadOnlySpan<char> lastRecordId = quotationRepository.GetLastRecordId();

            if (lastRecordId.IsEmpty)
            {
                return $"QT{currentYear}{currentMonth:00}{1:00000}";
            }

            ReadOnlySpan<char> monthFromId = lastRecordId.Slice(4, 2);

            // If the last record is from the current month, increment the running number
            int runningNum = currentMonth == int.Parse(monthFromId) ? int.Parse(lastRecordId.Slice(6, 5)) + 1 : 1;

            return $"QT{currentYear}{currentMonth:00}{runningNum:00000}";
        }
        public IActionResult DeleteItem(string quotationNo)
        {
            try
            {
                quotationRepository.DeleteQuotation(quotationNo);
                return RedirectToAction("QuotationList", "Quotation");
            }
            catch (SqlException)
            {
                return StatusCode(500);
            }

        }
        public JsonResult GetItemDetailById(string itemCode) => Json(itemRepository.GetItemById(itemCode));
    }
}
