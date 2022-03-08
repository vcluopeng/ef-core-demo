using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.ConsoleApp.Tests
{

    public class TestAggregate
    {
        public TestAggregate()
        {

        }


        [Benchmark(Baseline = true)]
        public async Task<string> NoGrouping()
        {
            using var dbContext = Program.CreateDbContext();
            var count = await dbContext.Users.Where(x => x.LastName == "��").CountAsync();
            var sumFromAge = await dbContext.Users.Where(x => x.LastName == "��").SumAsync(x => x.Age);
            var avgFromAge = await dbContext.Users.Where(x => x.LastName == "��").AverageAsync(x => x.Age);
            var maxFromAge = await dbContext.Users.Where(x => x.LastName == "��").MaxAsync(x => x.Age);
            var minFromAge = await dbContext.Users.Where(x => x.LastName == "��").MinAsync(x => x.Age);
            var sumFromPhoneLength = await dbContext.Users.Where(x => x.LastName == "��").SumAsync(x => x.Phone.Length);
            return "OK";
        }

        [Benchmark]
        public async Task<string> EmptyKeyGroup()
        {
            using var dbContext = Program.CreateDbContext();
            var summary = await dbContext.Users.Where(x => x.LastName == "��").GroupBy(x => 1).Select(x => new
            {
                Count = x.Count(),
                SumFromAge = x.Sum(u => u.Age),
                AvgFromAge = x.Average(u => u.Age),
                MaxFromAge = x.Max(u => u.Age),
                MinFromAge = x.Min(u => u.Age),
                SumFromPhoneLength = x.Sum(u => u.Phone.Length)
            }).FirstAsync();
            return "OK";
        }
    }
}
