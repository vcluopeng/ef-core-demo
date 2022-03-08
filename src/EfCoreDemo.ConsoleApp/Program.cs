using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using EfCoreDemo.ConsoleApp.Tests;
using EfCoreDemo.Core;
using EfCoreDemo.Core.Seeds;
using EfCoreDemo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.ConsoleApp
{
    public class Program
    {


        public static EfCoreDemoDbContext CreateDbContext(bool isMemory = EfCoreDemoConsts.UseInMemory)
        {
            return isMemory ? CreateDbContextInMemory() : CreateDbContextInSqlServer();
        }


        private static EfCoreDemoDbContext CreateDbContextInSqlServer()
        {
            var builder = new DbContextOptionsBuilder<EfCoreDemoDbContext>();
            EfCoreDemoDbContextConfigurer.Configure(builder, EfCoreDemoConsts.ConnectionString);
            return new EfCoreDemoDbContext(builder.Options);
        }

        private static EfCoreDemoDbContext CreateDbContextInMemory()
        {
            var builder = new DbContextOptionsBuilder<EfCoreDemoDbContext>();
            EfCoreDemoDbContextConfigurer.ConfigureAsInMemory(builder, "EfCoreDemoApp");
            return new EfCoreDemoDbContext(builder.Options);
        }


        static async Task Main(string[] args)
        {

            // BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, DefaultConfig.Instance
            //.WithSummaryStyle(new SummaryStyle(CultureInfo.InvariantCulture, printUnitsInHeader: false, SizeUnit.B, TimeUnit.Microsecond))
            //  );

            var job = Job.Dry.WithWarmupCount(EfCoreDemoConsts.Test_Warmup_Count).WithIterationCount(EfCoreDemoConsts.Test_Target_Count);


            var config = ManualConfig.CreateEmpty() // A configuration for our benchmarks
           .AddJob(job.WithRuntime(CoreRuntime.Core31))
           .AddJob(job.WithRuntime(CoreRuntime.Core50))
           .AddJob(job.WithRuntime(CoreRuntime.Core60))
           .AddColumn(TargetMethodColumn.Method, StatisticColumn.Max, StatisticColumn.Min, StatisticColumn.Mean, RankColumn.Arabic)
           .AddDiagnoser(new MemoryDiagnoser(new MemoryDiagnoserConfig(true)));




            if (EfCoreDemoConsts.UseInMemory)
            {
                await InitSeedData(500000);
            }
            else
            {
                Console.WriteLine("ʹ��SqlServer���ݿ�");
            }
            var summary = BenchmarkRunner.Run<TestNoTracking>(config);
            //var summary = BenchmarkRunner.Run<TestAny>(config);  
            //var summary = BenchmarkRunner.Run<TestLike>(config);
            //var summary = BenchmarkRunner.Run<TestAggregate>(config);
            //var summary = BenchmarkRunner.Run<TestBulkInsert>(config);
            //var summary = BenchmarkRunner.Run<TestBulkUpdate>(config);
            //var summary = BenchmarkRunner.Run<TestBulkDelete>(config);
        }

        static async Task InitSeedData(int count)
        {
            var sw = new Stopwatch();
            Console.WriteLine("ʹ���ڴ����ݿ�");
            Console.WriteLine("��ʼ����{0}����������...", count);
            sw.Start();
            using var dbContext = Program.CreateDbContextInMemory();
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await dbContext.Users.AddRangeAsync(UserSeedData.Build(count));
            await dbContext.SaveChangesAsync();
            sw.Stop();
            Console.WriteLine("��������׼�����,��ʱ:{0}", sw.ElapsedMilliseconds);
        }
    }
}
