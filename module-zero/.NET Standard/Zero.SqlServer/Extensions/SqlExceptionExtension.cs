using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace Zero.SqlServer.Extensions
{
    public static class SqlExceptionExtension
    {
        public static bool IsViolatesForeignKey(this SqlException exception)
        {
            if (exception.Number == 547 || exception.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
            {
                return true;
            }

            return false;
        }

        public static bool IsSqlViolatesForeignKey(this DbUpdateException exception)
        {
            if (exception?.InnerException is SqlException sqlException)
            {
                return sqlException.IsViolatesForeignKey();
            }

            return false;
        }

        public static bool IsSqlViolatesForeignKey(this Exception exception)
        {
            if (exception is DbUpdateException updateException)
            {
                return updateException.IsSqlViolatesForeignKey();
            }

            return false;
        }
    }
}