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

namespace EduTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BaseEduContext _baseEduContext;
        public HomeController(ILogger<HomeController> logger, BaseEduContext baseEduContext)
        {
            _logger = logger;
            _baseEduContext = baseEduContext;
        }
        public IActionResult Index()
        {
            var test = _baseEduContext.UserInfo.Where(x => x.Id != 0).ToList();
            _logger.LogInformation("胡汉三来了");
            _logger.LogError("座山雕也来了！");
            _logger.LogInformation($"mysql测试数据：{test}");
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
