using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem.ApplicationCore.Models.StockAsOnDetail
{
    public class StockAsOnDetailModel
    {        
        public string LabelId { get; set; }
        public string ItemCode { get; set; }
        public string LotNo { get; set; }
        public string LocationName { get; set; }
        public int Qty { get; set; }
        public string StockInDate { get; set; }

    }
}

