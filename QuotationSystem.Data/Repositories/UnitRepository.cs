using Microsoft.AspNetCore.Mvc.Rendering;
using QuotationSystem.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem.Data.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly DbContextOptionBuilder option;

        public UnitRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }
        public List<SelectListItem> GetAllUnitIds()
        {
            using (var db = new QuotationContext(option))
            {
                return db.MUnits.Select(d => new SelectListItem
                {
                    Text = d.UnitId,
                    Value = d.UnitId
                }).ToList();
            }
        }
    }
}
