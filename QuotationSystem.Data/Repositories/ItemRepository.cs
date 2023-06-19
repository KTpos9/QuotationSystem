using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zero.Core.Mvc.Models.DataTables;
using Zero.Extension;
using Zero.Core.Mvc.Extensions;

namespace QuotationSystem.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DbContextOptionBuilder option;

        public ItemRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }
        public List<MItem> GetAllItems()
        {
            using (var db = new QuotationContext(option))
            {
                return db.MItems.ToList();
            }
        }
        public MItem GetItemById(string itemCode)
        {
            using (var db = new QuotationContext(option))
            {
                return db.MItems.Find(itemCode);
            }
        }
        public DataTableResultModel<MItem> GetItemList(DataTableOptionModel dtOption, string itemCode = "", string itemName = "")
        {
            using (var db = new QuotationContext(option))
            {
                return db.MItems
                    .WhereIf(string.IsNullOrEmpty(itemCode) == false, x => x.ItemCode.Contains(itemCode))
                    .WhereIf(string.IsNullOrEmpty(itemName) == false, x => x.ItemName.Contains(itemName))
                    .Select(i => new MItem
                    {
                        ItemCode = i.ItemCode,
                        ItemName = i.ItemName,
                        ItemDesc = i.ItemDesc,
                        UnitPrice = i.UnitPrice,
                        Unit = i.Unit,
                        ActiveStatus = i.ActiveStatus
                    })
                    .ToDataTableResult(dtOption);
            }
        }
        public void EditItem(MItem item, string currentUser = "Admin")
        {
            using (var db = new QuotationContext(option))
            {
                var itemToUpdate = db.MItems.Find(item.ItemCode);
                itemToUpdate.ItemName = item.ItemName;
                itemToUpdate.UnitPrice = item.UnitPrice;
                itemToUpdate.Unit = item.Unit;
                itemToUpdate.Remark = item.Remark;
                itemToUpdate.ItemDesc = item.ItemDesc;

                db.CurrentUser = currentUser;
                db.SaveChanges();
            }
        }
        public void DeleteItem(string itemCode, string currentUser = "Admin")
        {
            using (var db = new QuotationContext(option))
            {
                var item = db.MUsers.Find(itemCode);
                db.Remove(item);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// it actually add item by a list of objects
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task AddItemByExcel(List<MItem> items, string currentUser = "Admin")
        {
            using (var db = new QuotationContext(option))
            {
                await db.Database.ExecuteSqlRawAsync("DELETE FROM t_quotation_detail");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM m_item");
                await db.AddRangeAsync(items);
                db.CurrentUser = currentUser;
                db.SaveChanges();
            }
        }
    }
}
