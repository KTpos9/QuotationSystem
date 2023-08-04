using QuotationSystem.Data.Models;

namespace QuotationSystem.Data.Repositories.Interfaces
{
    public interface IStockRepository
    {
        bool addStockIn(TStock inStock);
    }
}