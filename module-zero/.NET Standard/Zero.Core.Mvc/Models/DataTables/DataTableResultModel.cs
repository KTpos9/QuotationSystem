using System.Collections.Generic;

namespace Zero.Core.Mvc.Models.DataTables
{
    public class DataTableResultModel<T> where T : class, new()
    {
        public List<T> Data { get; set; }
        public int Rows { get; set; }
    }
}