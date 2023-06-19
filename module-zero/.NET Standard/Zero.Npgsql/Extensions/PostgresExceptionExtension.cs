using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Zero.Npgsql.Extensions
{
    public static class PostgresExceptionExtension
    {
        public static bool IsViolatesForeignKey(this PostgresException exception)
        {
            if (exception.SqlState == "23503")
            {
                return true;
            }

            return false;
        }

        public static bool IsPostgresViolatesForeignKey(this DbUpdateException exception)
        {
            if (exception.InnerException is PostgresException)
            {
                return (exception.InnerException as PostgresException).IsViolatesForeignKey();
            }

            return false;
        }
    }
}