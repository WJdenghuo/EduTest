using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EduTest.Controllers
{
    [Controller]
    public class TRTCController : Controller
    {
        public IActionResult Index()
        {
            StringBuilder builder = new StringBuilder();
            return View();
        }
        public IActionResult RTCDemo()
        {
            return View();
        }
    }
}