using QuotationSystem.ApplicationCore.Models.StockAsOnDetail;
using QuotationSystem.Data.Models;
using System.Linq;
using Zero.Core.Mvc.Models.DataTables;

namespace QuotationSystem.Data.Repositories.Interfaces
{
    public interface IStockRepository
    {
        (bool isValid, string exMessage) addStockIn(TStock inStock);
        IQueryable<TStock> GetStockList(string itemCode = "", string whId = "");
        IQueryable<StockAsOnDetailModel> GetLabelList(string itemCode, string whId);
    }
}