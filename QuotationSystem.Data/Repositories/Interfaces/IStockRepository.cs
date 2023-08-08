using QuotationSystem.Data.Models;
using System.Linq;
using Zero.Core.Mvc.Models.DataTables;

namespace QuotationSystem.Data.Repositories.Interfaces
{
    public interface IStockRepository
    {
        bool addStockIn(TStock inStock);
        IQueryable<TStock> GetStockList(DataTableOptionModel dtOption, string itemCode = "", string whId = "");

    }
}