using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using EfCoreDemo.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.ConsoleApp.Tests
{

    public class TestBulkDelete
    {
        [Params(8000)]
        public int Count { get; set; }
        public TestBulkDelete()
        {

        }


        [Benchmark(Baseline = true)]
        public async Task<int> AutoDetectChangesOn()
        {
            using var dbContext = Program.CreateDbContext();
            var users = await dbContext.Users.Take(Count).ToArrayAsync();
            dbContext.Users.RemoveRange(users);
            var count = await dbContext.SaveChangesAsync();
            return count;
        }

        [Benchmark]
        public async Task<int> AutoDetectChangesOff()
        {
            using var dbContext = Program.CreateDbContext();
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            var users = await dbContext.Users.Take(Count).ToArrayAsync();
            dbContext.Users.RemoveRange(users);
            var count = await dbContext.SaveChangesAsync();
            return count;
        }

        [Benchmark]
        public async Task<int> NoQuery()
        {
            using var dbContext = Program.CreateDbContext();
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            var userId = await dbContext.Users.Take(Count).Select(x => x.Id).ToArrayAsync();//模拟从数据加载需要的数据主键(正常业务是从前端得到他)
            var users = userId.Select(x => new User() { Id = x }).ToArray();
            dbContext.Users.RemoveRange(users);
            var count = await dbContext.SaveChangesAsync();
            return count;
        }
    }
}
