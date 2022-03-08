using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using EfCoreDemo.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.ConsoleApp.Tests
{

    public class TestBulkUpdate
    {
        [Params(8000)]
        public int Count { get; set; }
        public int Seed { get; set; }
        public TestBulkUpdate()
        {

        }

        [GlobalSetup]
        public void Setup()
        {
            Seed = new Random().Next(10, 999);
        }


        [Benchmark(Baseline = true)]
        public async Task<int> AutoDetectChangesOn()
        {
            using var dbContext = Program.CreateDbContext();
            var users = await dbContext.Users.Take(Count).ToArrayAsync();
            users.AsParallel().ForAll(user =>
            {
                user.Phone = $"3325151{Seed}";
                user.Age = 340 + Seed;
            });
            var count = await dbContext.SaveChangesAsync();
            return count;
        }

        [Benchmark]
        public async Task<int> AutoDetectChangesOff()
        {
            using var dbContext = Program.CreateDbContext();
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            var users = await dbContext.Users.Take(Count).ToArrayAsync();
            users.AsParallel().ForAll(user =>
            {
                user.Phone = $"4435151{Seed}";
                user.Age = 350 + Seed;
            });
            dbContext.ChangeTracker.DetectChanges();
            var count = await dbContext.SaveChangesAsync();
            return count;
        }

        [Benchmark]
        public async Task<int> NoQuery()
        {
            using var dbContext = Program.CreateDbContext();
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            var userId = await dbContext.Users.Take(Count).Select(x => x.Id).ToArrayAsync();//模拟从数据加载需要的数据主键(正常业务是从前端得到他)
            var users = userId.Select(x =>
            {
                var user = new User() { Id = x };
                dbContext.Users.Attach(user);
                user.Phone = $"5545151{Seed}";
                user.Age = 360 + Seed;
                return user;
            }).ToArray();
            dbContext.ChangeTracker.DetectChanges();
            var count = await dbContext.SaveChangesAsync();
            return count;
        }
    }

}
