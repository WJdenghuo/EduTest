using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EduTest.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Edu.Entity.MySqlEntity;
using Microsoft.EntityFrameworkCore;
using Edu.Entity;
using NLog.Targets;
using NLog.Conditions;
using Edu.Tools.Helper;
using UserInfo = Edu.Entity.MySqlEntity.UserInfo;

namespace EduTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly BaseEduContext _baseEduContext;
        public HomeController(ILogger<HomeController> logger, BaseEduContext baseEduContext)
        {
            _logger = logger;
            _baseEduContext = baseEduContext;
        }      
        public async Task<IActionResult>  Index()
        {
            NLog.LogEventInfo ei = new NLog.LogEventInfo(NLog.LogLevel.Debug, _logger.GetType().Name, $"{User.Identity.Name}:写入数据库测试！");
            ei.Properties["Application"] = "test";
            var test =await _baseEduContext.UserInfo.AsNoTracking().Where(x => x.Id != 0).ToListAsync();
            RedisHelper redisHelper = new RedisHelper("127.0.0.1:6379");

            redisHelper.HashSet("RightsTable", test, R =>
            {
                return R.Id.ToString();
            });
            var RsultList = redisHelper.HashGetAll<UserInfo>("RightsTable");


            //自定义参数存在问题，待调试
            _logger.LogError("颜色测试", ei);

            #region 测试
            //var consoleTarget = new ColoredConsoleTarget();
            //var highlightRule = new ConsoleRowHighlightingRule();
            //highlightRule.Condition = ConditionParser.ParseExpression("level == LogLevel.Info");
            //highlightRule.ForegroundColor = ConsoleOutputColor.Green;
            //consoleTarget.RowHighlightingRules.Add(highlightRule);
            //_logger.LogTrace("控制台测试");
            #endregion

            //异常错误的捕获需要添加全局异常捕获
            return View();
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
