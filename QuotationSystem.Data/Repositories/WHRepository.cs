using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem.Data.Repositories
{
    public class WHRepository : IWHRepository
    {
        private DbContextOptionBuilder option;

        public WHRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }

        public List<string> GetAllWHIds()
        {
            using (var db = new QuotationContext(option))
            {
                return db.MWhs.Select(d => d.WhId).ToList();
            }
        }
    }
}
