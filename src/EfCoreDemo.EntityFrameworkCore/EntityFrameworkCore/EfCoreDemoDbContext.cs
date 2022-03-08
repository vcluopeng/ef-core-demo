using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfCoreDemo.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.EntityFrameworkCore
{
    public class EfCoreDemoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public EfCoreDemoDbContext(DbContextOptions<EfCoreDemoDbContext> options) : base(options)
        {

        }
    }
}
