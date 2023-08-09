using Microsoft.EntityFrameworkCore;
using QuotationSystem.ApplicationCore.Models.StockAsOnDetail;
using QuotationSystem.Data.Helpers;
using QuotationSystem.Data.Models;
using QuotationSystem.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zero.Core.Mvc.Extensions;
using Zero.Core.Mvc.Models.DataTables;
using Zero.Extension;

namespace QuotationSystem.Data.Repositories
{
    public class StockRepository : IStockRepository
    {
        private DbContextOptionBuilder option;

        public StockRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }

        public (bool isValid, string exMessage) addStockIn(TStock inStock)
        {
            using (var db = new QuotationContext(option))
            {
                try {
                    db.Add(inStock);
                    db.SaveChanges();
                    return (isValid:true, exMessage:"");
                }
                catch(Exception ex) {
                Console.WriteLine(ex.Message);
                    if(ex.InnerException != null)
                    {
                        if (ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                        {
                            return (isValid: false, exMessage: "Cannot insert duplicate Label ID");
                        };
                        if (ex.InnerException.Message.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint"))
                        {
                            return (isValid: false, exMessage: "Not Found Location ID ");
                        };
                    }
                    return (isValid: false, exMessage: ex.InnerException.Message);
                }
                
            }
        }

        public IQueryable<TStock> GetStockList(string itemCode = "", string whId = "")
        {
            var db = new QuotationContext(option);
            var itemNames = db.MItems
                            .WhereIf(!string.IsNullOrEmpty(itemCode), x => x.ItemCode.Contains(itemCode))
                            .Select(x => new { x.ItemCode, x.ItemName });
            var stock = db.TStocks
                        .Include(s => s.ItemCodeNavigation)
                        .WhereIf(!string.IsNullOrEmpty(whId), x => x.WhId == whId)
                        .WhereIf(!string.IsNullOrEmpty(itemCode), x => x.ItemCode.Contains(itemCode))
                        .GroupBy(x => new { x.WhId, x.ItemCode })
                        .Select(x => new TStock
                        {
                            WhId = x.Key.WhId,
                            ItemCode = x.Key.ItemCode,
                            Qty = x.Sum(y => y.Qty),
                            ItemCodeNavigation = itemNames.Where(y => y.ItemCode == x.Key.ItemCode).Select(y => new MItem
                            {
                                ItemName = y.ItemName
                            }).FirstOrDefault()
                        });
            return stock;
        }



        public IQueryable<StockAsOnDetailModel> GetLabelList(string itemCode, string whId)
        {
            var db = new QuotationContext(option);
            var labelList = db.TStocks
                            .WhereIf(!string.IsNullOrEmpty(whId), x => x.WhId == whId)
                            .WhereIf(!string.IsNullOrEmpty(itemCode), x => x.ItemCode.Contains(itemCode))
                            .Join(db.MLocations, stock => stock.LocationId, location => location.LocationId,
                                (stock, location) => new StockAsOnDetailModel
                                {
                                    LabelId = stock.LabelId,
                                    ItemCode = stock.ItemCode,
                                    LotNo = stock.LotNo,
                                    LocationName = location.LocationName,
                                    Qty = stock.Qty,
                                    StockInDate = stock.StockInDate.ToString("dd/MM/yyyy") // Format the date as needed
                                });

            return labelList;
        }
    



}
}
