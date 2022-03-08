using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using EfCoreDemo.Core.Entities;

namespace EfCoreDemo.ConsoleApp.Tests
{
    public class TestBulkInsert
    {
        [Params(8000)]
        public int Count { get; set; }
        private readonly bool _useInMemory = true;
        public TestBulkInsert()
        {

        }


        [Benchmark(Baseline = true)]
        public async Task<int> AutoDetectChangesOn()
        {
            var users = Build().ToArray();
            using var dbContext = Program.CreateDbContext(_useInMemory);
            await dbContext.Users.AddRangeAsync(users);
            foreach (var item in users)
            {
                //模拟业务场景
                if (item.FirstName.Length <= 2)
                {
                    item.FirstName += item.FirstName;
                }
            }
            var count = await dbContext.SaveChangesAsync();
            return count;
        }

        [Benchmark]
        public async Task<int> AutoDetectChangesOff()
        {
            var users = Build().ToArray();
            using var dbContext = Program.CreateDbContext(_useInMemory);
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await dbContext.Users.AddRangeAsync(users);
            foreach (var item in users)
            {
                //模拟业务场景
                if (item.FirstName.Length <= 2)
                {
                    item.FirstName += item.FirstName;
                }
            }
            var count = await dbContext.SaveChangesAsync();
            return count;
        }

        public IEnumerable<User> Build()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return new User()
                {
                    FirstName = "大河",
                    LastName = "李",
                    FullName = "大河 李",
                    UserName = "UserName" + i,
                    Email = $"1326512{i}@163.com",
                    Phone = "1326512" + i,
                    Age = 20 + i,
                    NickName = "NickName" + i
                };
            }
        }
    }
}
