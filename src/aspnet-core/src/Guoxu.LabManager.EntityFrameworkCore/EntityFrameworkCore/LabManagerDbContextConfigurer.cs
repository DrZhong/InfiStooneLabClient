using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Guoxu.LabManager.EntityFrameworkCore
{
    public static class LabManagerDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<LabManagerDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<LabManagerDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
