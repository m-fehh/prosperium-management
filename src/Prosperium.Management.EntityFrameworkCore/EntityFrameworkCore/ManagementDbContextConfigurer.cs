using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Prosperium.Management.EntityFrameworkCore
{
    public static class ManagementDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ManagementDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ManagementDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
