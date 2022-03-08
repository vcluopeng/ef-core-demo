using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using EfCoreDemo.Core;
using EfCoreDemo.Core.Dto;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.ConsoleApp.Tests
{

    public class TestLike
    {
        private readonly string _keyword = EfCoreDemoConsts.KeyWord;
        private readonly string _containsKeyword = "%" + EfCoreDemoConsts.KeyWord + "%";
        public TestLike()
        {

        }

        [Benchmark(Baseline = true)]
        public async Task<int> Contains()
        {
            using var dbContext = Program.CreateDbContext();
            return await dbContext.Users.Where(x => x.FullName.Contains(_keyword)).CountAsync();
        }

        [Benchmark]
        public async Task<int> Like()
        {
            using var dbContext = Program.CreateDbContext();
            return await dbContext.Users.Where(x => EF.Functions.Like(x.FullName, _containsKeyword)).CountAsync();
        }

        [Benchmark]
        public async Task<int> FulltextIndex()
        {
            using var dbContext = Program.CreateDbContext();
            return await dbContext.Users.Where(x => EF.Functions.Contains(x.FullName, _keyword)).CountAsync();
        }

    }
}
