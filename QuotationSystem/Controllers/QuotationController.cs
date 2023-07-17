using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Models.Quotation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;

namespace QuotationSystem.Controllers
{
    public class QuotationController : Controller
    {
        private readonly IQuotationRepository quotationRepository;
        public QuotationController(IQuotationRepository quotationRepository)
        {
            this.quotationRepository = quotationRepository;
        }
        public IActionResult QuotationList()
        {
            return View();
        }
        public IActionResult PreviewQuotation(string quotationNo)
        {
            var model = quotationRepository.GetQuotationById(quotationNo);

            return View(model);
        }
        [HttpPost]
        public IActionResult AddQuotation(QuotationViewModel model, List<QuotationItemViewModel> itemList)
        {
            try
            {
                var quotationHeader = new TQuotationHeader
                {
                    QuotationNo = model.QuotationNo,
                    QuotationDate = model.Date,
                    CustomerName = model.Customer,
                    CustomerAddress = model.CustomerAddress,
                    Seller = model.SalesName,
                    Vat = model.Vat,
                    ActiveStatus = model.ActiveStatus switch
                    {
                        "on" => "Y",
                        _ => "N"
                    }
                    ,
                    Total = itemList.Sum(item => double.Parse(item.unitPrice)),
                    TQuotationDetails = itemList.Select(item => new TQuotationDetail
                    {
                        ItemCode = item.itemCode,
                        ItemQty = item.Qty,
                        DiscountPercent = item.discount / 100,
                        Remark = item.itemDesc
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
        public JsonResult Search(string quotationNo, string customer, DataTableOptionModel option)
        {
            var result = quotationRepository.GetQuotationList(option, qutoationNo: quotationNo, customer: customer);
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
        public IActionResult EditQuotation(QuotationViewModel quotationModel)
        {
            //quotationRepository.EditQuotation(quotationModel.QuotationHeader);
            return RedirectToAction("QuotationList");
        }
    }
}
