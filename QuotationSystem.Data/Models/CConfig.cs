using System;
using System.Collections.Generic;

#nullable disable

namespace QuotationSystem.Data.Models
{
    public partial class CConfig
    {
        public string ConfCode { get; set; }
        public string ConfName { get; set; }
        public string ConfValue { get; set; }
        public string ConfDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}
