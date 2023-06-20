using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Models.Quotation;
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
        public IActionResult AddQuotation(QuotationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var quotationHeader = new TQuotationHeader
                {
                    QuotationNo = model.QuotationNo,
                    QuotationDate = model.Date,
                    CustomerName = model.Customer,
                    CustomerAddress = model.CustomerAddress,
                    Seller = model.SalesName,
                    Vat = model.Vat,
                    ActiveStatus = model.ActiveStatus,
                    GrandTotal = model.ItemList.Sum(x => x.UnitPrice) * (100 + model.Vat)/100
                };
                var quotationDetail = new TQuotationDetail
                {
                    ItemQty = model.ItemList.Count,
                    ActiveStatus = model.ActiveStatus,
                    
                    QuotationNoNavigation = quotationHeader
                };
                quotationRepository.AddQuotation(quotationDetail);
                return RedirectToAction("QuotationList", "Quotation");
            }
            return ViewComponent("");
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
            return RedirectToAction("ItemList");
        }
        public IActionResult QuotationPreview()
        {
            return View();
        }
    }
}
