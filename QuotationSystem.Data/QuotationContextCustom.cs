using QuotationSystem.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotationSystem.Data
{
    public partial class QuotationContext
    {
        public QuotationContext(DbContextOptionBuilder options) : base(options.Build())
        {

        }
    }
}
