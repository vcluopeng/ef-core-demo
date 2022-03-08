using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.ConsoleApp.Tests
{

    public class TestAny
    {
        private readonly int _count = 20;
        public TestAny()
        {

        }


        [Benchmark(Baseline = true)]
        public async Task<string> Count()
        {
            for (int i = 0; i < _count; i++)
            {
                using var dbContext = Program.CreateDbContext();
                var count = await dbContext.Users.Where(x => x.LastName == "��").CountAsync();
                Empty(count > 0);
            }
            return "OK";
        }

        [Benchmark]
        public async Task<string> FirstOrDefault()
        {
            for (int i = 0; i < _count; i++)
            {
                using var dbContext = Program.CreateDbContext();
                var user = await dbContext.Users.Where(x => x.LastName == "��").FirstOrDefaultAsync();
                Empty(user != null);
            }
            return "OK";
        }

        [Benchmark]
        public async Task<string> Any()
        {
            for (int i = 0; i < _count; i++)
            {
                using var dbContext = Program.CreateDbContext();
                var exists = await dbContext.Users.AnyAsync(x => x.LastName == "��");
                Empty(exists);
            }
            return "OK";
        }

        [Benchmark]
        public async Task<string> AnyVar()
        {
            for (int i = 0; i < _count; i++)
            {
                using var dbContext = Program.CreateDbContext();
                var lastName = "��";
                var exists = await dbContext.Users.AnyAsync(x => x.LastName == lastName);
                Empty(exists);
            }
            return "OK";
        }
        public bool Empty(bool exists)
        {
            //TODO:ģ��ҵ���߼�
            if (exists == false)
            {
                Console.WriteLine("û�з���������������");
            }
            return exists == true;

        }
    }
}
