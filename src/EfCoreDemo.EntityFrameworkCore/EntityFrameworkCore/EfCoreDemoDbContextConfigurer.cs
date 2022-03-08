using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.EntityFrameworkCore
{
    public static class EfCoreDemoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EfCoreDemoDbContext> builder, string connectionString)
        {
            //builder.UseSqlServer(connectionString, options => options.UseRelationalNulls(true));
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<EfCoreDemoDbContext> builder, DbConnection connection)
        {
            // builder.UseSqlServer(connection, options => options.UseRelationalNulls(true));
            builder.UseSqlServer(connection);
        }

        public static void ConfigureAsInMemory(DbContextOptionsBuilder<EfCoreDemoDbContext> builder, string databaseName)
        {
            builder.UseInMemoryDatabase(databaseName);
        }
    }
}