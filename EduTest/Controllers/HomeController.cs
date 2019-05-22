using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EduTest.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Edu.Entity.MySqlEntity;
using Microsoft.EntityFrameworkCore;
using UserInfo = Edu.Entity.MySqlEntity.UserInfo;
using Edu.Tools.Redis;
using StackExchange.Redis;

namespace EduTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        private readonly BaseEduContext _baseEduContext;
        private readonly ConnectionMultiplexer _redis;
        public HomeController(ILogger<HomeController> logger, BaseEduContext baseEduContext, ConnectionMultiplexer connectionMultiplexer)
        {
            _logger = logger;
            _baseEduContext = baseEduContext;
            _redis = connectionMultiplexer;
        }      
        public async Task<IActionResult>  Index()
        {
            NLog.LogEventInfo ei = new NLog.LogEventInfo(NLog.LogLevel.Debug, _logger.GetType().Name, $"{User.Identity.Name}:写入数据库测试！");
            ei.Properties["Application"] = "test";
            var test =await _baseEduContext.UserInfo.AsNoTracking().Where(x => x.Id != 0).ToListAsync();

            #region Redis测试
            //改封装存在较多问题，后续会参照源码调整
            //RedisHelper redisHelper = new RedisHelper("127.0.0.1:6379");
            //redisHelper.HashSet("RightsTable", test, R =>
            //{
            //    return R.Id.ToString();
            //});
            //var RsultList = redisHelper.HashGetAll<UserInfo>("RightsTable");

            var redisHelper = _redis.GetDatabase();
            redisHelper.StringSet("搜神记", @"
                                            刹那芳华  --树下野狐

                                            朝露昙花，咫尺天涯，
                                            人道是黄河十曲，毕竟东流去。
                                            八千年玉老，一夜枯荣，问苍天此生何必？
                                            昨夜风吹处，落英听谁细数？
                                            九万里苍穹，御风弄影，水人与共？
                                            千秋北斗，瑶宫寒苦，不若神仙眷侣，百年江湖。");
            var gs = redisHelper.StringGet("搜神记");
            #endregion



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
