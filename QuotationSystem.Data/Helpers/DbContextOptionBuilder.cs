using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuotationSystem.Data.Models;
using System;

namespace QuotationSystem.Data.Helpers
{
    public class DbContextOptionBuilder
    {
        private readonly DbContextOptionsBuilder<QuotationContext> builder;

        public DbContextOptionBuilder(string connectionString)
        {
            builder = new DbContextOptionsBuilder<QuotationContext>();
            builder.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information);
        }

        internal DbContextOptions Build()
        {
            return builder.Options;
        }
    }
}