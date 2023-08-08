using Microsoft.EntityFrameworkCore;
using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;
using Zero.Extension;

namespace QuotationSystem.Data.Repositories
{
    public class StockRepository : IStockRepository
    {
        private DbContextOptionBuilder option;

        public StockRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }

        public bool addStockIn(TStock inStock)
        {
            using (var db = new QuotationContext(option))
            {
                try {
                    db.Add(inStock);
                    db.SaveChanges();
                    return true;
                }
                catch(Exception ex) {
                Console.WriteLine(ex.Message);
                    return false;
                }
                
            }
        }

        public DataTableResultModel<TStock> GetStockList(DataTableOptionModel dtOption, string itemCode = "", string whId = "")
        {
            using(var db = new QuotationContext(option))
            {
                var stock = db.TStocks.Include(t => t.ItemCodeNavigation)
                    .WhereIf(!string.IsNullOrEmpty(itemCode), x => x.ItemCode.Contains(itemCode))
                    .WhereIf(!string.IsNullOrEmpty(whId), x => x.ItemCode.Contains(whId))
                    .Select(s => new TStock
                    {
                        ItemCode = s.ItemCode,
                        WhId = s.WhId,
                        Qty = s.Qty,
                        ItemCodeNavigation = s.ItemCodeNavigation
                    })
                    .ToDataTableResult(dtOption);
                return stock;
            }   
        }
    }
}
