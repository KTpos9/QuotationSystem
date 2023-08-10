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
    [Authorize(Policy = Policy.QuotationManagement)]
    public class QuotationController : Controller
    {
        private readonly IQuotationRepository quotationRepository;
        private readonly IConfigRepository configRepository;
        private readonly IItemRepository itemRepository;
        private readonly ISessionContext sessionContext;

        private string CurrentUser;
        public QuotationController(IQuotationRepository quotationRepository, IConfigRepository configRepository, ISessionContext sessionContext, IItemRepository itemRepository)
        {
            this.quotationRepository = quotationRepository;
            this.configRepository = configRepository;
            this.sessionContext = sessionContext;
            CurrentUser = sessionContext.CurrentUser.Id;
            this.itemRepository = itemRepository;
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
                double sumOfItemPrice = itemList.Sum(item => double.Parse(item.total));
                var quotationHeader = new TQuotationHeader
                {
                    QuotationNo = model.QuotationNo,
                    QuotationDate = DateTime.UtcNow,
                    CustomerName = model.Customer,
                    CustomerAddress = model.CustomerAddress,
                    CustomerContact = model.CustomerContact,
                    TaxId = model.TaxId,
                    Seller = model.SalesName,
                    Vat = model.Vat,
                    CreateBy = CurrentUser,
                    UpdateBy = CurrentUser,
                    ActiveStatus = model.ActiveStatus,
                    Total = itemList.Sum(item => double.Parse(item.unitPrice)),
                    GrandTotal = sumOfItemPrice * (1 + (model.Vat / 100)),
                    TQuotationDetails = itemList.Select(item => new TQuotationDetail
                    {
                        ItemCode = item.itemCode,
                        ItemQty = item.Qty,
                        DiscountPercent = item.discount / 100,
                        Remark = item.itemDesc,
                        CreateBy = CurrentUser,
                        UpdateBy = CurrentUser
                    }).ToList()
                };
                quotationRepository.AddQuotation(quotationHeader);
                return Ok(new { isSuccess = true });
            }
            catch (SqlException)
            {
                return StatusCode(500);
            }
        }
        public JsonResult Search(string quotationNo, string customer, string startDate, string endDate, DataTableOptionModel option)
        {
            var result = quotationRepository.GetQuotationList(option, 
                                                              qutoationNo: quotationNo, 
                                                              customer: customer, startDate: 
                                                              startDate, 
                                                              endDate: endDate);
            var response = result.ToJsonResult(option);
            return response;
        }
        public PartialViewResult GetEditQuotationModal(string itemCode)
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
        [HttpPost]
        public IActionResult EditQuotation(QuotationViewModel quotationModel, List<QuotationItemViewModel> itemList)
        {
            try
            {
                double sumOfItemPrice = itemList.Sum(item => double.Parse(item.total));
                var quotationHeader = new TQuotationHeader
                {
                    QuotationNo = quotationModel.QuotationNo,
                    QuotationDate = DateTime.UtcNow,
                    CustomerName = quotationModel.Customer,
                    CustomerAddress = quotationModel.CustomerAddress,
                    CustomerContact = quotationModel.CustomerContact,
                    TaxId = quotationModel.TaxId,
                    Seller = quotationModel.SalesName,
                    Vat = quotationModel.Vat,
                    CreateBy = CurrentUser,
                    UpdateDate = DateTime.UtcNow,
                    UpdateBy = CurrentUser,
                    ActiveStatus = quotationModel.ActiveStatus,
                    Total = sumOfItemPrice,
                    GrandTotal = sumOfItemPrice * (1 + (quotationModel.Vat / 100)),
                    TQuotationDetails = itemList.Select(item => new TQuotationDetail
                    {
                        ItemCode = item.itemCode,
                        ItemQty = item.Qty,
                        DiscountPercent = item.discount / 100,
                        CreateBy = CurrentUser,
                        UpdateDate = DateTime.UtcNow,
                        UpdateBy = CurrentUser
                    }).ToList()
                };
                quotationRepository.EditQuotation(quotationHeader);
                return Ok(new { isSuccess = true });
            }
            catch(Exception e)
            {
                return StatusCode(500, new { isSuccess = false, exception = e });
            }
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
        public IActionResult DeleteQuotation(string quotationNo)
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
