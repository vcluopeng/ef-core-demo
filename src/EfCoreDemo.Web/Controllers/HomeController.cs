using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using efCoreDemo.Models;
using EfCoreDemo.Core;
using EfCoreDemo.Core.Dto;
using EfCoreDemo.Core.Entities;
using EfCoreDemo.Core.Seeds;
using EfCoreDemo.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;

namespace efCoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EfCoreDemoDbContext _efCoreDemoDbContext;


        private static readonly Func<EfCoreDemoDbContext, int, Task<User>> _userQuery = EF.CompileAsyncQuery((EfCoreDemoDbContext context, int age) => context.Users.Where(u => u.Age == age).FirstOrDefault());
        private readonly string _keyword = EfCoreDemoConsts.KeyWord;
        private readonly string _startWithKeyword = EfCoreDemoConsts.KeyWord + "%";
        private readonly string _containsKeyword = "%" + EfCoreDemoConsts.KeyWord + "%";
        private readonly IMapper _mapper;


        public HomeController(ILogger<HomeController> logger, EfCoreDemoDbContext efCoreDemoDbContext, IMapper mapper)
        {
            _logger = logger;
            _efCoreDemoDbContext = efCoreDemoDbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> AnyAsync()
        {
            await _efCoreDemoDbContext.Users.AsNoTracking().FirstAsync();



            var profiler = MiniProfiler.Current;
            var sw = new Stopwatch();

            sw.Start();
            using (profiler.Step("使用Count"))
            {
                var resultForCountt = await _efCoreDemoDbContext.Users.Where(x => x.LastName == "罗").CountAsync();
                Debug.WriteLineIf(resultForCountt > 0, nameof(resultForCountt));
            }
            sw.Stop();
            _logger.LogWarning("使用{Keyword}:{ElapsedMilliseconds}ms@{Id}", "Count", sw.ElapsedMilliseconds, profiler.Id);


            sw.Restart();
            using (profiler.Step("使用FirstOrDefault"))
            {
                var resultForFirstOrDefault = await _efCoreDemoDbContext.Users.FirstOrDefaultAsync(x => x.LastName == "罗");
                Debug.WriteLineIf(resultForFirstOrDefault != null, nameof(resultForFirstOrDefault));
            }
            sw.Stop();
            _logger.LogWarning("使用{Keyword}:{ElapsedMilliseconds}ms@{Id}", "FirstOrDefault", sw.ElapsedMilliseconds, profiler.Id);


            sw.Restart();
            using (profiler.Step("使用FirstOrDefault+AsNoTracking"))
            {
                var resultForAsNoTracking = await _efCoreDemoDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.LastName == "罗");
                Debug.WriteLineIf(resultForAsNoTracking != null, nameof(resultForAsNoTracking));
            }
            sw.Stop();
            _logger.LogWarning("使用{Keyword}:{ElapsedMilliseconds}ms@{Id}", "FirstOrDefault+AsNoTracking", sw.ElapsedMilliseconds, profiler.Id);


            sw.Restart();
            using (profiler.Step("使用Any"))
            {
                var resultForAny = await _efCoreDemoDbContext.Users.AnyAsync(x => x.LastName == "罗");
                Debug.WriteLineIf(resultForAny, nameof(resultForAny));
            }
            sw.Stop();
            _logger.LogWarning("使用{Keyword}:{ElapsedMilliseconds}ms@{Id}", "Any", sw.ElapsedMilliseconds, profiler.Id);


            sw.Restart();
            using (profiler.Step("使用Any+变量"))
            {
                var lastName = "罗";
                var resultForAnyLastName = await _efCoreDemoDbContext.Users.AnyAsync(x => x.LastName == lastName);
                Debug.WriteLineIf(resultForAnyLastName, nameof(resultForAnyLastName));
            }
            sw.Stop();
            _logger.LogWarning("使用{Keyword}:{ElapsedMilliseconds}ms@{Id}", "使用Any+变量", sw.ElapsedMilliseconds, profiler.Id);


            return View("Test");
        }

        public async Task<IActionResult> LikeAsync()
        {
            await _efCoreDemoDbContext.Users.AsNoTracking().FirstAsync();
            var profiler = MiniProfiler.Current;


            using (profiler.Step("使用Contains"))
            {
                var resultForStartsWith = await _efCoreDemoDbContext.Users.Where(x => x.FullName.Contains(_keyword)).CountAsync();
            }


            using (profiler.Step($"使用{_containsKeyword}"))
            {
                var resultForStartsWith = await _efCoreDemoDbContext.Users.Where(x => EF.Functions.Like(x.FullName, _containsKeyword)).CountAsync();
            }

            using (profiler.Step($"使用{_startWithKeyword}"))
            {
                var resultForStartsWith = await _efCoreDemoDbContext.Users.Where(x => EF.Functions.Like(x.FullName, _startWithKeyword)).CountAsync();
            }

            using (profiler.Step("使用全文索引Contains"))
            {
                var resultForStartsWith = await _efCoreDemoDbContext.Users.Where(x => EF.Functions.Contains(x.FullName, _keyword)).CountAsync();
            }



            return View("Test");
        }

        public async Task<SimpleUser[]> UseMapper()
        {
            var userQuery = _efCoreDemoDbContext.Users.Where(x => x.Age > 18);
            //优化前:太多属性需要赋值
            var users = userQuery.Select(x => new SimpleUser()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FullName = x.FullName
            }).ToArrayAsync();

            //优化后:使用AutoMapper的ProjectTo方法转换IQueryable
            var simpleUserQuery = _mapper.ProjectTo<SimpleUser>(userQuery);

            return await simpleUserQuery.ToArrayAsync();
        }

        public IActionResult Get()
        {
            return View();
        }

        public async Task<IActionResult> GetById(int id = 5214863)
        {
            var user = await _efCoreDemoDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Json(user);
        }

        [HttpPut]
        public async Task UpdateAsync(int id, string email, string nickname)
        {
            _efCoreDemoDbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            var user = new User()
            {
                Id = id
            };
            _efCoreDemoDbContext.Users.Attach(user);
            user.Email = email;
            if (!string.IsNullOrWhiteSpace(nickname))
            {
                user.NickName = nickname;
            }
            _efCoreDemoDbContext.ChangeTracker.DetectChanges();
            await _efCoreDemoDbContext.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task DeleteAsync(int id)
        {
            _efCoreDemoDbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            var user = new User()
            {
                Id = id
            };
            _efCoreDemoDbContext.Users.Remove(user);
            await _efCoreDemoDbContext.SaveChangesAsync();
        }



        [HttpDelete]
        public async Task DeleteAllAsync(int[] ids)
        {
            _efCoreDemoDbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            //例1:根据前端传递的主键删除user
            var users = ids.Select(x => new User()
            {
                Id = x
            });
            foreach (var user in users)
            {
                _efCoreDemoDbContext.Users.Remove(user);
            }
            _efCoreDemoDbContext.ChangeTracker.DetectChanges();
            await _efCoreDemoDbContext.SaveChangesAsync();


            //例2:把未成年的user全部删除
            var minAge = 18;
            var userIds = await _efCoreDemoDbContext.Users.Where(x => x.Age < minAge).Select(x => x.Id).ToArrayAsync();
            var removeUsers = userIds.Select(x => new User()
            {
                Id = x
            });
            foreach (var user in removeUsers)
            {
                _efCoreDemoDbContext.Users.Attach(user);
                _efCoreDemoDbContext.Users.Remove(user);
            }
            await _efCoreDemoDbContext.SaveChangesAsync();


            await _efCoreDemoDbContext.Users.Where(x => x.Age < minAge).Select(x => x.Id).ToArrayAsync();

        }


        public async Task<IActionResult> InitAsync(int count = 1000)
        {
            _efCoreDemoDbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _efCoreDemoDbContext.Users.AddRangeAsync(UserSeedData.Build(count));
            await _efCoreDemoDbContext.SaveChangesAsync();
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
