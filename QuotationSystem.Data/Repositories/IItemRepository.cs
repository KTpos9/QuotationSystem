using QuotationSystem.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zero.Core.Mvc.Models.DataTables;

namespace QuotationSystem.Data.Repositories
{
    public interface IItemRepository
    {
        List<MItem> GetAllItems();
        MItem GetItemById(string itemCode);
        DataTableResultModel<MItem> GetItemList(DataTableOptionModel dtOption, string itemCode = "", string itemName = "");
        void EditItem(MItem item, string currentUser = "Admin");
        void DeleteItem(string itemCode, string currentUser = "Admin");
        Task AddItemByExcel(List<MItem> items, string currentUser = "Admin");
    }
}