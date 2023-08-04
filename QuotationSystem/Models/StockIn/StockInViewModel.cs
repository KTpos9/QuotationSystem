using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuotationSystem.Models.StockIn
{
    public class StockInViewModel
    {
        [RegularExpression(@"^\d{4}(0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])\d{5}$", ErrorMessage = "Invalid Label ID format.")]
        [Required(ErrorMessage = "Label ID is required.")]
        public string LabelId { get; set; }
        [Required(ErrorMessage = "Item Code is required.")]
        public string ItemCode { get; set; }
        [Required(ErrorMessage = "Item Name is required.")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "Lot No is required.")]
        public string LotNo { get; set; }
        [Required(ErrorMessage = "Qty is required.")]
        public int Qty { get; set; }
        [Required(ErrorMessage = "Warehouse ID is required.")]
        public string WhId { get; set; }
        [Required(ErrorMessage ="Location ID is required.")]
        public string LocationId { get; set; }
        public List<string> WhIds { get; set; }

    }
}
