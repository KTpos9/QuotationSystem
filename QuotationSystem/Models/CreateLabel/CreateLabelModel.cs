using System.ComponentModel.DataAnnotations;

namespace QuotationSystem.Models.CreateLabel
{
    public class CreateLabelModel
    {
        [Required(ErrorMessage = "Item Code is required.")]
        [StringLength(30, MinimumLength = 9, ErrorMessage = "Item Code must be 9 characters.")]
        public string ItemCode { get; set; }

        [Required(ErrorMessage = "Item not found.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Item Name must be between 1 and 30 characters.")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Lot No is required.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Lot No must be between 1 and 30 characters.")]
        public string LotNo { get; set; }

        [Required(ErrorMessage = "Qty is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Qty must be a positive number.")]
        public int Qty { get; set; }

        [Required(ErrorMessage = "Label Qty is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Label Qty must be a positive number.")]
        public int LabelQty { get; set; }

    }
}
