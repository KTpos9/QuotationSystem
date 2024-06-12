using QuotationSystem.Data.Models;

namespace QuotationSystem.Data.Repositories.Interfaces
{
    public interface IRunningNoRepository
    {
        string GetLastRunningDate();
        string GetLastRunningNo();
        void insert(TRunningNo runningNo);
        void Update(string runningDate, int labelQty);
    }
}