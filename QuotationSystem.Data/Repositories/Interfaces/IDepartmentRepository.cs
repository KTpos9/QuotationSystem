using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace QuotationSystem.Data.Repositories
{
    public interface IDepartmentRepository
    {
        List<SelectListItem> GetAllDepartmentIds();
    }
}