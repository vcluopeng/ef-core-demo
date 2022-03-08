using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using EfCoreDemo.Core.Dto;
using EfCoreDemo.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.ConsoleApp.Tests
{
    public class TestNoTracking
    {
        [Params(8000)]
        public int Count { get; set; }
        public TestNoTracking()
        {

        }


        [Benchmark(Baseline = true)]
        public async Task<List<User>> AsTracking()
        {
            using var dbContext = Program.CreateDbContext();
            var userResult = await dbContext.Users.Take(Count).ToListAsync();
            var s = string.Empty;
            foreach (var item in userResult)
            {
                //模拟业务代码
                var length = item.FirstName.Length;
                if (length <= 2)
                {
                    s += item.FirstName;
                }
            }
            return userResult;
        }

        [Benchmark]
        public async Task<List<User>> AsNoTracking()
        {
            using var dbContext = Program.CreateDbContext();
            var userResult = await dbContext.Users.AsNoTracking().Take(Count).ToListAsync();
            var s = string.Empty;
            foreach (var item in userResult)
            {
                //模拟业务代码
                var length = item.FirstName.Length;
                if (length <= 2)
                {
                    s += item.FirstName;
                }
            }
            return userResult;
        }


        [Benchmark]
        public async Task<SimpleUser[]> UseDto()
        {
            using var dbContext = Program.CreateDbContext();
            var resultForArray = await dbContext.Users.Take(Count).Select(x => new SimpleUser(x.Id, x.FirstName, x.LastName, x.FullName)).ToArrayAsync();
            var s = string.Empty;
            foreach (var item in resultForArray)
            {
                //模拟业务代码
                var length = item.FirstName.Length;
                if (length <= 2)
                {
                    s += item.FirstName;
                }
            }
            return resultForArray;
        }

    }
}
