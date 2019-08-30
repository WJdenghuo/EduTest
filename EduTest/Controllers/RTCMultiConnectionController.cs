using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.Controllers
{
    public class RTCMultiConnectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ScreenShare()
        {
            return View();
        }
    }
}