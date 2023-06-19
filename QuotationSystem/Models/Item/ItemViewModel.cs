using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotationSystem.Models.Item
{
    public class ItemViewModel
    {
        public List<MItem> Items { get; set; }
        public MItem Item { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string ItemCode { get; set; }
    }
}
