using QuotationSystem.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem.Data.Repositories
{
    public class ConfigRepository : IConfigRepository
    {
        private DbContextOptionBuilder option;
        public ConfigRepository(DbContextOptionBuilder option)
        {
            this.option = option;
        }
        public string GetDefaultPassowrd()
        {
            using (var db = new QuotationContext(option))
            {
                var config = db.CConfigs.FirstOrDefault(c => c.ConfCode == "C001");
                return config.ConfValue;
            }
        }
        public string GetConfigById(string id)
        {
            using (var db = new QuotationContext(option))
            {
                return db.CConfigs.FirstOrDefault(c => c.ConfCode == id).ConfValue;
            }
        }
    }
}
