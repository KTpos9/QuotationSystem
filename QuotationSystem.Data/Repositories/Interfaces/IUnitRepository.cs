using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace QuotationSystem.Data.Repositories
{
    public interface IUnitRepository
    {
        List<SelectListItem> GetAllUnitIds();
    }
}