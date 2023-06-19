using Microsoft.AspNetCore.Mvc.Rendering;
using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zero.Core.Mvc.Models.Select2;

namespace QuotationSystem.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DbContextOptionBuilder option;
        public DepartmentRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }
        public List<SelectListItem> GetAllDepartmentIds()
        {
            using (var db = new QuotationContext(option))
            {
                return db.MDepartments.Select(d => new SelectListItem
                {
                    Text = d.DepartmentId,
                    Value = d.DepartmentId
                }).ToList();
            }
        }
    }
}
