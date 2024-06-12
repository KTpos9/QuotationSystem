using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotationSystem.Models.Quotation
{
    public class PreviewQuotationViewModel
    {
        public TQuotationHeader Quotation { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyTaxId { get; set; }
    }
}
