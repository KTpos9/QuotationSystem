using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuotationSystem.Models.StockIn
{
    public class StockInModel
    {
        // check reg-ex in format "labelid,itemcode,lotno,qty"
        [Required(ErrorMessage = "Item Code is required.")]
        public string LabelId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string LotNo { get; set; }
        public string Qty { get; set; }
        public string WhId { get; set; }

        [Required(ErrorMessage ="Location ID is required.")]
        public string LocationId { get; set; }
        public List<string> WhIds { get; set; }

    }
}
