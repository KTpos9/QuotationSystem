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
                        UnitId = i.UnitId,
                        ActiveStatus = i.ActiveStatus
                    })
                    .ToDataTableResult(dtOption);
            }
        }
        public void EditItem(MItem item, string currentUser)
        {
            using (var db = new QuotationContext(option))
            {
                var itemToUpdate = db.MItems.Find(item.ItemCode);
                itemToUpdate.ItemName = item.ItemName;
                itemToUpdate.UnitPrice = item.UnitPrice;
                itemToUpdate.Unit = item.Unit;
                itemToUpdate.Remark = item.Remark;
                itemToUpdate.ItemDesc = item.ItemDesc;
                itemToUpdate.ActiveStatus = item.ActiveStatus switch
                {
                    "false" => "N",
                    _ => "Y"
                };
                itemToUpdate.UpdateDate = DateTime.UtcNow;
                itemToUpdate.UpdateBy = currentUser;

                db.SaveChanges();
            }
        }
        public void DeleteItem(string itemCode)
        {
            using (var db = new QuotationContext(option))
            {
                try
                {
                    var item = new MItem() { ItemCode = itemCode };
                    var itemToDelete = db.MItems.Attach(item);
                    itemToDelete.State = EntityState.Deleted;

                    db.SaveChanges();
                }
                catch(DbUpdateConcurrencyException)
                {
                    return;
                }
            }
        }
        /// <summary>
        /// it actually add item by a list of objects
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task AddItemByExcel(List<MItem> items)
        {
            using (var db = new QuotationContext(option))
            {
                //await db.Database.ExecuteSqlRawAsync("DELETE FROM t_quotation_detail");
                //await db.Database.ExecuteSqlRawAsync("DELETE FROM m_item");
                db.AddRange(items);
                db.SaveChanges();
            }
        }
    }
}
