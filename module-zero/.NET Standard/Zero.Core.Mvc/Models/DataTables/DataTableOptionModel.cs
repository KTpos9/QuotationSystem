using System.Collections.Generic;
using System.Linq;

namespace Zero.Core.Mvc.Models.DataTables
{
    public class DataTableOptionModel
    {
        // properties are not capital due to json mapping
        public int draw { get; set; }

        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }

        public int GetDisplayStart()
        {
            return start;
        }

        public string GetOrderOptions()
        {
            if (order != null)
            {
                var ordering = order.Select(x => "[" + x.column + ",'" + x.dir + "']").ToList();
                var result = "[" + string.Join(",", ordering) + "]";
                return result;
            }

            return "[]";
        }

        public class Column
        {
            public string data { get; set; }
            public string name { get; set; }
            public bool searchable { get; set; }
            public bool orderable { get; set; }
            public Search search { get; set; }
        }

        public class Search
        {
            public string value { get; set; }
            public string regex { get; set; }
        }

        public class Order
        {
            public int column { get; set; }
            public string dir { get; set; }
        }
    }
}