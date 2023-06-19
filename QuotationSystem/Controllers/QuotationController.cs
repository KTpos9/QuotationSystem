using Microsoft.AspNetCore.Mvc;
using QuotationSystem.Data.Repositories;
using QuotationSystem.Models.Quotation;
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
        public JsonResult Search(string quotationNo, string customer, DataTableOptionModel option)
        {
            var result = quotationRepository.GetQuotationList(option, qutoationNo: quotationNo, customer: customer);
            var response = result.ToJsonResult(option);
            return response;
        }
        public PartialViewResult GetEditItemModal(string itemCode)
        {
            var quotation = quotationRepository.GetQuotationById(itemCode);
            var model = new QuotationViewModel
            {
                QuotationHeader = quotation
            };
            return PartialView("_EditItemPartial", model);
        }
        public PartialViewResult GetDeleteItemModal(string quotationNo)
        {
            var model = new QuotationViewModel
            {
                QuotationNo = quotationNo
            };
            return PartialView("_DeleteItemPartial", model);
        }
        public IActionResult EditItem(QuotationViewModel quotationModel)
        {
            quotationRepository.EditQuotation(quotationModel.QuotationHeader);
            return RedirectToAction("ItemList");
        }
        public IActionResult QuotationPreview()
        {
            return View();
        }
    }
}
