using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class MLocation
    {
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string ActiveStatus { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
