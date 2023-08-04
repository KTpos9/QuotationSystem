using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
