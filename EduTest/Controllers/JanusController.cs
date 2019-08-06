using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EduTest.Controllers
{
    public class JanusController : Controller
    {
        private readonly ILogger _logger;
        public JanusController(ILogger<JanusController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Room()
        {
            return View();
        }
        public IActionResult RoomTest()
        {
            //异常筛选器测试
            try
            {

            }
            catch (Exception e) when (LogException(e))
            {

                throw;
            }
            return View();
        }
        public IActionResult RoomOnlySubscribeTest()
        {
            return View();
        }
        public IActionResult RoomLiveTest()
        {
            return View();
        }
        public Boolean LogException(Exception e)
        {
            _logger.LogError(e,e.Message,nameof(JanusController));
            return false;
        }
    }
}