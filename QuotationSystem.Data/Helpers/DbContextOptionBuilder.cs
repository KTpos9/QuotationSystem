using Microsoft.EntityFrameworkCore;
using QuotationSystem.Data.Models;

namespace QuotationSystem.Data.Helpers
{
    public class DbContextOptionBuilder
    {
        private readonly DbContextOptionsBuilder<QuotationContext> builder;

        public DbContextOptionBuilder(string connectionString)
        {
            builder = new DbContextOptionsBuilder<QuotationContext>();
            builder.UseSqlServer(connectionString);
        }

        internal DbContextOptions Build()
        {
            return builder.Options;
        }
    }
}