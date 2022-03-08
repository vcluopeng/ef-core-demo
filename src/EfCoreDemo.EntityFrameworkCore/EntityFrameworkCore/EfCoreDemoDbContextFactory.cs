using System;
using System.IO;
using EfCoreDemo.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EfCoreDemo.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class EfCoreDemoDbContextFactory : IDesignTimeDbContextFactory<EfCoreDemoDbContext>
    {
        public EfCoreDemoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<EfCoreDemoDbContext>();

            /*
             You can provide an environmentName parameter to the AppConfigurations.Get method. 
             In this case, AppConfigurations will try to read appsettings.{environmentName}.json.
             Use Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") method or from string[] args to get environment if necessary.
             https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#args
             */

            var configuration = AppConfigurations.Get(Path.Combine(Environment.CurrentDirectory, "../EfCoreDemo.Web"));

            EfCoreDemoDbContextConfigurer.Configure(builder, configuration.GetConnectionString(EfCoreDemoConsts.ConnectionStringName));

            return new EfCoreDemoDbContext(builder.Options);
        }
    }
}
