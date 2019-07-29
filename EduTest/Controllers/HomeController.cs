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
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using Aspose.Pdf;

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
            redisHelper.StringSet("诸葛亮——王司徒——饶舌", @"

                                王朗：久闻公之大名，今日有幸相会。公既知天命，识时务，为何要兴无名之师，犯我疆界？
                                孔明：我奉诏讨贼，何谓之无名？
                                王朗：天数有变，神器更易，而归有德之人， 此乃自然之理。
                                孔明：曹贼篡汉，霸占中原，何称有德之人？
                                王朗：
                                自桓帝、灵帝以来，黄巾猖獗，天下纷争，社稷累卵之危，生灵有倒悬之急。
                                我太祖武皇帝，扫清六合，席卷八荒，万姓倾心，四方仰德，此非以权势取之，实乃天命所归也！
                                我世祖文皇帝，神文圣武，继承大统，应天合人，法尧禅舜，处中国以治万邦，这岂非天心人意乎？
                                今公蕴大才，抱大器，自比管仲、乐毅，何乃强要逆天理，背人情而行事？
                                岂不闻古人云：顺天者昌，逆天者亡？
                                今我大魏，带甲百万，良将千员。
                                谅尔等腐草之荧光，如何比得上天空之皓月？
                                你若倒戈卸甲，以礼来降，仍不失封侯之位，国安民乐，岂不美哉？
                                孔明：
                                哈哈哈哈……
                                我原以为你身为汉朝老臣，来到阵前，面对两军将士，必有高论，没想到竟说出如此粗鄙之语！
                                我有一言，请诸位静听。
                                昔日桓帝、灵帝之时，汉统衰落，宦官酿祸，国乱岁凶，四方扰攘。
                                黄巾之后，董卓、李傕、郭汜等接踵而起，劫持汉帝，残暴生灵。
                                因之，庙堂之上，朽木为官，殿陛之间，禽兽食禄。
                                以至狼心狗行之辈汹汹当朝，奴颜婢膝之徒纷纷秉政。
                                以致社稷变为丘墟，苍生饱受涂炭之苦！
                                值此国难之际，王司徒又有何作为？
                                王司徒之生平，我素有所知。
                                你世居东海之滨，初举孝廉入仕，理当匡君辅国，安汉兴刘，何期反助逆贼，同谋篡位！
                                罪恶深重，天地不容！
                                王朗：你...你... 诸葛村夫……你敢……
                                孔明：
                                住口！
                                无耻老贼，岂不知天下之人，皆愿生啖你肉，安敢在此饶舌！
                                今幸天意不绝炎汉，昭烈皇帝于西川，继承大统。
                                我今奉嗣君之旨，兴师讨贼，你既为谄谀之臣，只可潜身缩首，苟图衣食，怎敢在我军面前妄称天数！
                                皓首匹夫，苍髯老贼！
                                你即将命归九泉之下，届时有何面目去见汉朝二十四代先帝！
                                王朗：我……我……
                                孔明：
                                二臣贼子，你枉活七十有六，一生未立寸功，只会摇唇鼓舌，助曹为虐！
                                一条断脊之犬，还敢在我军阵前狺狺狂吠。
                                王朗：我……我……
                                孔明：我从未见过有如此厚颜无耻之人");
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

            //异常错误的捕获需要添加全局异常捕获，在mvc中添加异常和表单验证的过滤器。
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
        public bool OkCookie()
        {
            var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
            consentFeature.GrantConsent();
            return true;
        }
        public IActionResult AsposePDF()
        {
            using (var stream=new MemoryStream())
            {
                var pdf = new Aspose.Pdf.Document(stream);
                pdf.Save("", SaveFormat.Html);
            }
            
            return View();
        }
    }
}
